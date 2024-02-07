namespace WebApplication2.Models
{
    public class Option
    {
        public int OptionId { get; set; }

        public int LigneDecommandeId { get; set; }
        public LigneDeCommande? LigneDeCommande { get; set; }

        public int IngredientId { get; set; }
        public Ingredient? Iingredient { get; set; }

       
    }
}
