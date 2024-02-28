using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Compte
    {
        public int CompteId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Le {0} doit être au moins {2} caractères de long.", MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
   
        public  Client? Client { get; set; }
    }
}

