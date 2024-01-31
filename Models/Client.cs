namespace WebApplication2.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }

        public IList<Commande> Commandes { get; set;}
    }
}
