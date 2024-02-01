using WebApplication2.Models;

namespace WebApplication2.ViewModel
{
    public class PizzaViewModel
    {
        public Pizza Pizza { get; set; }
        public List<Ingredient> IngredientsDisponible { get; set; }
    }
}
