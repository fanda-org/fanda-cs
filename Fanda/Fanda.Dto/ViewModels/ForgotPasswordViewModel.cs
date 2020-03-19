using System.ComponentModel.DataAnnotations;

namespace Fanda.Dto.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}