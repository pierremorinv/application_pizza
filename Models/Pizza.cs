using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplication2.Models
{

    public class Pizza
    {
        [JsonIgnore]
        public int PizzaId { get; set; }
        [Display(Name = "Nom")]
        public string Nom { get; set; }
        [Display(Name = "prix(€)")]
        [Column(TypeName = "decimal(18, 2)")]
        public float Prix { get; set; }
        [Display(Name = "Végétarienne")]
        public bool Vegetarienne { get; set; }

        public IList <Ingredient>?  Ingredients { get; set; }

        public LigneDeCommande? LigneDeCommande { get; set; }
    }
}
