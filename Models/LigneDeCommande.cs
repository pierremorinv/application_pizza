using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class LigneDeCommande
    {
        [Key]
        public int LigneDeCommandeId { get; set; }

        // relations one to many entre Commande et ligneDeCommande
        public Commande? Commande { get; set; }
        public int CommandeId { get; set; }

        // relation one to Many entre Pizza et LigneDeCommande
        public Pizza Pizza { get; set; } = null;

        public int PizzaId { get; set; }
      

        public int QuantitePizza { get; set; } 

        public float PrixUnitaire { get; set; }

      
        // relation Many to Many entre Ingredients et LigneDeCommande
        public IList <Ingredient>? Ingredients { get; set; }

        public bool Vegetarien {  get; set; }
    
    }
}
