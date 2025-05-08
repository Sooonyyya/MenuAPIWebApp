using System.ComponentModel.DataAnnotations;

namespace MenuAPIWebApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } // зберігаємо хеш пароля, а не сам пароль

        // Можна додати ще більше полів, наприклад:
        // public string Role { get; set; } // Користувач, Адміністратор
    }
}
