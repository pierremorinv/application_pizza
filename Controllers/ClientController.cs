using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.ViewModel;

namespace WebApplication2.Controllers
{
    [Authorize]
    public class ClientController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index(string ClientId)
        {

            if (ClientId == null)
            {
                return NotFound();
            }

            return View(await _context.Clients
                .Include(cl => cl.Commandes).ThenInclude(c => c.ligneDeCommandes).ThenInclude(lc => lc.Pizza)
                .Include(cl => cl.Commandes).ThenInclude(c => c.ligneDeCommandes).ThenInclude(lc => lc.Ingredients)
                .AsNoTracking().FirstOrDefaultAsync(c => c.ClientId == ClientId));
        }

        public async Task<IActionResult> NewCommande(string ClientId)
        {
            Commande commande = new Commande()
            {
                ClientID = ClientId,
                DateCommande = DateTime.Now,

            };
            _context.Commandes.Add(commande);
            _context.SaveChanges();

            return RedirectToAction("", "menu", commande.CommandeId);
        }

   

     

    }
  
}
