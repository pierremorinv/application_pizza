using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Ingredient
    {

        public int IngredientId { get; set; }


        public required string Nom { get; set; }
        public bool Vegetarien { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public float Prix { get; set; }

        public IList<Pizza>? Pizzas { get; set; }


    }
}
