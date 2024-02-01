namespace WebApplication2.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string ClientFirstName { get; set; }

        public string ClientLastName { get; set; }

        public IList<Commande>? Commandes { get; set; }

    }
}
