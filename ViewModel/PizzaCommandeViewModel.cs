using WebApplication2.Models;

namespace WebApplication2.ViewModel
{
    public class PizzaCommandeViewModel
    {
        public IList<Pizza>? Pizzas { get; set; }

        public Commande? Commande { get; set; }

        public IList<Ingredient> ingredients { get; set; }

        public Pizza Pizza { get; set; }

        public IList<LigneDeCommande> LigneDeCommandes { get; set; }


    }
}
