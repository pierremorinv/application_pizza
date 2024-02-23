using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.ViewModel;

namespace WebApplication2.Controllers
{
    public class ClientController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index(int ClientId)
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

        public async Task<IActionResult> NewCommande(int ClientId)
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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            Client client = new Client();
            return View("Create",client);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Client client)
        {
            
            Client clientEnBase = await _context.Clients.FirstOrDefaultAsync(c => c.ClientFirstName == client.ClientFirstName && c.ClientLastName == client.ClientLastName);
            if (client == null)
            {
                return (NotFound());
            }

            if (clientEnBase != null) {

                TempData["Error"] = "Ce Compte existe déja";
                return RedirectToAction("Create");
            }
            else if (client.ClientLastName.Length < 2)
            {
                TempData["Error"] = "Choissisez un nom plus grand ";
                return View();
            }
            else if (client.ClientFirstName.Length < 2)
            {
                TempData["Error"] = "Choissisez un prénom plus grand ";
                return View();
            }

            else
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", client);
            }

           
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            Client client = new Client();
            return View("Login", client);
        }


        [HttpPost]
        public async Task<IActionResult> Login(Client client)
        {
            Client clientEnBase = await _context.Clients.FirstOrDefaultAsync(c => c.ClientFirstName == client.ClientFirstName && c.ClientLastName == client.ClientLastName);
            if (client == null)
            {
                return (NotFound());
            }

            if (clientEnBase == null)
            {

                TempData["Error"] = "Ce Compte n'existe pas ";
                return View();
            }
            
            else
            {
               
                return RedirectToAction("Index", clientEnBase);
            }


        }
    }
  
}
