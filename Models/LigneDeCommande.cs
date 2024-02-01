using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class LigneDeCommande
    {
        [Key]
        public int LigneDeCommandeId { get; set; }

        public int PizzaId { get; set; }
        public Pizza Pizza { get; set; } = null;

        public int PrixUnitaire { get; set; }

        public Commande Commande { get; set; }
        public int CommandeId { get; set; }

    }
}
