using System.ComponentModel.DataAnnotations;

namespace NoteMangmentApp.API.ViewModels
{
    public class RegisterVM
    {
        [Required,MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
