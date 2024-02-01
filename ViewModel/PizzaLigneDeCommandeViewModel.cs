using WebApplication2.Models;

namespace WebApplication2.ViewModel
{
    public class PizzaLigneDeCommandeViewModel
    {
        public IList<Pizza>? Pizzas { get; set; }

        public LigneDeCommande? LigneDeCommande { get; set; }
    }
}
