namespace WebApplication2.Models
{
    public class LigneDeCommande
    {
        public int LigneCommandeId { get; set; }

        public int PizzaId { get; set; }
        public Pizza Pizza { get; set; }

        public int PrixUnitaire { get; set; }

    }
}
