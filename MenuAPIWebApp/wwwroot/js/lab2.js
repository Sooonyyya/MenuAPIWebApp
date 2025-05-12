// lab2.js

const API_BASE = "/api";
let currentTypeId = null;
let favoriteIds = [];
let allDishes = [];
const sessionId = localStorage.getItem("sessionId") || crypto.randomUUID();
localStorage.setItem("sessionId", sessionId);

window.onload = async function () {
    await loadDishTypes();
    await loadDishes();
    await loadFavorites();
    loadAddDishButton();
    addHeartStyle();
};

function addHeartStyle() {
    const style = document.createElement("style");
    style.textContent = `
        .heart {
            transition: transform 0.3s ease, color 0.3s ease;
            cursor: pointer;
        }
        .heart.animate {
            transform: scale(1.4);
        }
    `;
    document.head.appendChild(style);
}

async function openDishForm(id = null) {
    document.getElementById("edit-dish-id").value = id || "";
    document.getElementById("edit-dish-name").value = "";
    document.getElementById("edit-dish-description").value = "";
    document.getElementById("edit-dish-price").value = "";
    document.getElementById("edit-dish-calories").value = "";

    const typeSelect = document.getElementById("edit-dish-type");
    const res = await fetch("/api/DishTypes");
    const types = await res.json();
    typeSelect.innerHTML = types.map(t => `<option value="${t.id}">${t.name}</option>`).join("");

    const ingContainer = document.getElementById("edit-dish-ingredients");
    const ires = await fetch("/api/Ingredients");
    const ings = await ires.json();
    ingContainer.innerHTML = ings.map(i => `
        <label><input type="checkbox" value="${i.id}"> ${i.name}</label><br>`).join("");

    document.getElementById("edit-dish-title").textContent = id ? "Редагувати страву" : "Нова страва";
    document.getElementById("edit-dish-dialog").showModal();
}

async function saveDish() {
    const id = document.getElementById("edit-dish-id").value;
    const name = document.getElementById("edit-dish-name").value.trim();
    const description = document.getElementById("edit-dish-description").value.trim();
    const price = parseFloat(document.getElementById("edit-dish-price").value);
    const calories = parseInt(document.getElementById("edit-dish-calories").value);
    const dishTypeId = parseInt(document.getElementById("edit-dish-type").value);
    const ingredientIds = Array.from(document.querySelectorAll("#edit-dish-ingredients input:checked")).map(i => parseInt(i.value));

    const body = {
        Id: parseInt(id) || 0,
        Name: name,
        Description: description,
        Price: price,
        Calories: calories,
        DishTypeId: dishTypeId,
        IngredientIds: ingredientIds
    };

    const method = id ? "PUT" : "POST";
    const url = id ? `${API_BASE}/Dishes/${id}` : `${API_BASE}/Dishes`;

    await fetch(url, {
        method,
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body)
    });

    document.getElementById("edit-dish-dialog").close();
    await loadDishes();
    await loadFavorites();
}


async function loadDishTypes() {
    const response = await fetch(`${API_BASE}/DishTypes`);
    const types = await response.json();
    const container = document.getElementById("dish-types");
    container.innerHTML = "";

    const allBtn = document.createElement("button");
    allBtn.innerText = "Усі";
    allBtn.classList.add("active");
    allBtn.onclick = () => {
        currentTypeId = null;
        document.querySelectorAll("#dish-types button").forEach(btn => btn.classList.remove("active"));
        allBtn.classList.add("active");
        loadDishes();
    };
    container.appendChild(allBtn);

    types.forEach(t => {
        const btn = document.createElement("button");
        btn.innerText = t.name;
        btn.onclick = () => {
            currentTypeId = t.id;
            document.querySelectorAll("#dish-types button").forEach(btn => btn.classList.remove("active"));
            btn.classList.add("active");
            loadDishes(t.id);
        };
        container.appendChild(btn);
    });
}

async function loadDishes() {
    const response = await fetch(`${API_BASE}/Dishes`);
    allDishes = await response.json();
    const dishes = currentTypeId ? allDishes.filter(d => d.dishTypeId === currentTypeId) : allDishes;

    const list = document.getElementById("dish-list");
    list.innerHTML = "";

    dishes.forEach(d => {
        const isFavorite = favoriteIds.includes(d.id);
        const card = document.createElement("div");
        card.className = "dish-card";
        card.innerHTML = `
            <h3>${d.name}</h3>
            <p>${d.description || ""}</p>
            <p><strong>${d.price.toFixed(2)}</strong> грн</p>
            <span class="heart ${isFavorite ? "active" : ""}" data-id="${d.id}" style="color: ${isFavorite ? 'red' : 'gray'}">&#10084;</span>
            <button onclick="showDish(${d.id})">Деталі</button>
            <button onclick="editDish(${d.id})">Редагувати</button>
            <button onclick="deleteDish(${d.id})">Видалити</button>
        `;
        const heart = card.querySelector(".heart");
        heart.onclick = async () => {
            const toggled = await toggleFavorite(d.id);
            heart.classList.toggle("active", toggled);
            heart.style.color = toggled ? "red" : "gray";
            heart.classList.add("animate");
            setTimeout(() => heart.classList.remove("animate"), 300);
        };
        list.appendChild(card);
    });
}

// (інший код залишається без змін)


async function loadFavorites() {
    const response = await fetch(`${API_BASE}/FavoriteDishes/${sessionId}`);
    const dishes = await response.json();
    favoriteIds = dishes.map(d => d.id);

    const favList = document.getElementById("favorite-dishes");
    if (favList) {
        favList.innerHTML = "";
        dishes.forEach(d => {
            const el = document.createElement("div");
            el.className = "dish-card";
            el.innerHTML = `<h3>${d.name}</h3><p>${d.description}</p>`;
            favList.appendChild(el);
        });
    }
}

async function toggleFavorite(id) {
    const isFav = favoriteIds.includes(id);
    const url = `${API_BASE}/FavoriteDishes/${sessionId}/${id}`;

    if (isFav) {
        console.log("🗑 Видаляємо з обраного:", id);
        const res = await fetch(url, { method: "DELETE" });
        console.log("⬅ DELETE відповідь:", res.status);
        favoriteIds = favoriteIds.filter(f => f !== id);
    } else {
        console.log("➕ Додаємо в обране:", { UserSessionId: sessionId, DishId: id });

        const res = await fetch(`${API_BASE}/FavoriteDishes`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ UserSessionId: sessionId, DishId: id })
        });

        console.log("⬅ POST відповідь:", res.status);
        if (res.ok) favoriteIds.push(id);
        else {
            const err = await res.text();
            console.error("❌ Помилка додавання:", err);
        }
    }

    console.log("📋 Актуальний favoriteIds:", favoriteIds);
    await loadFavorites();
    return !isFav;
}



async function deleteDish(id) {
    await fetch(`${API_BASE}/Dishes/${id}`, { method: "DELETE" });
    await loadDishes();
    await loadFavorites();
}

async function showDish(id) {
    const res = await fetch(`${API_BASE}/Dishes/${id}`);
    const dish = await res.json();

    document.getElementById("dish-dialog-title").textContent = dish.name;
    document.getElementById("dish-description").textContent = dish.description || "-";
    document.getElementById("dish-price").textContent = dish.price.toFixed(2);
    document.getElementById("dish-calories").textContent = dish.calories;
    document.getElementById("dish-ingredients").textContent = dish.dishIngredients.map(di => di.ingredient.name).join(", ");

    const reviews = document.getElementById("dish-reviews");
    reviews.innerHTML = dish.reviews.map(r => `<p><strong>${r.userName}</strong>: ${r.text}</p>`).join("");

    document.getElementById("send-review").onclick = async () => {
        const name = document.getElementById("review-name").value.trim();
        const text = document.getElementById("review-text").value.trim();
        if (!name || !text) return;

        await fetch(`${API_BASE}/Reviews`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ DishId: dish.id, UserName: name, Text: text })
        });

        document.getElementById("review-name").value = "";
        document.getElementById("review-text").value = "";
        showDish(dish.id);
    };

    document.getElementById("close-dialog").onclick = () => {
        document.getElementById("dish-dialog").close();
    };

    document.getElementById("dish-dialog").showModal();
}

function loadAddDishButton() {
    document.getElementById("add-dish-button").onclick = async () => {
        await openDishForm();
    };
    document.getElementById("cancel-edit").onclick = () => {
        document.getElementById("edit-dish-dialog").close();
    };
    document.getElementById("dish-form").onsubmit = async (e) => {
        e.preventDefault();
        await saveDish();
    };
}
