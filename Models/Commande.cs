namespace WebApplication2.Models
{
    public class Commande
    {
        public int CommandeId { get; set; }

        public DateTime DateCommande { get; set; }

        public Client  Client {  get; set; }

        public int ClientID { get; set; }
    }
}
