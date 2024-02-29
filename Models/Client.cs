

namespace WebApplication2.Models
{
    public class Client
    {
        public string ClientId { get; set; }

        public IList<Commande>? Commandes { get; set; }

        //public string ClientFirstName { get; set; }

        //public string ClientLastName { get; set; }

        public int CompteId { get; set; }

        public Compte Compte { get; set; } = null!;




    }
}
