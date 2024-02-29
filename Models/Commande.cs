namespace WebApplication2.Models
{
    public class Commande
    {
        public int CommandeId { get; set; }

        public DateTime DateCommande { get; set; }

        public Client  Client {  get; set; }

        public string ClientID { get; set; }

        public IList<LigneDeCommande> ligneDeCommandes { get; set; }

        public float? PrixTotal { get; set; }
    }
}
