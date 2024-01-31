namespace WebApplication2.Models
{
    public class Commande
    {
        public int CommandeId { get; set; }

        public DateTime DateTime { get; set; }

        public Client  client {  get; set; }
    }
}
