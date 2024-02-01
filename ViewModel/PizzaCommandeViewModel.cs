using WebApplication2.Models;

namespace WebApplication2.ViewModel
{
    public class PizzaCommandeViewModel
    {
        public IList<Pizza>? Pizzas { get; set; }

        public IList <Commande>? Commandes { get; set; }

        public IList<LigneDeCommande>? LigneDeCommandes { get; set; }
    }
}
