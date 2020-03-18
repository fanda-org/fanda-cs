using System.ComponentModel.DataAnnotations;

namespace Fanda.Dto
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}