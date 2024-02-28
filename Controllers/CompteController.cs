using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication2.ViewModel;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace WebApplication2.Controllers
{
    public class CompteController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Compte compte, Client client)
        {
            if (compte == null)
            {
                return NotFound();
            }
            Compte CompteEnBase = await _context.Comptes.FirstOrDefaultAsync(c => c.Password == compte.Password && c.Email == compte.Email);
            Compte CompteAvecMauvaisMotDePasse = await _context.Comptes.FirstOrDefaultAsync(c => c.Email == compte.Email && c.Password != compte.Password);

            if (CompteEnBase != null)
            {
                TempData["Error"] = "Ce Compte existe déja";
                return RedirectToAction("Index");
            }
            if (CompteAvecMauvaisMotDePasse != null)
            {
                TempData["Error"] = "Mot de passe incorrect";
                return RedirectToAction("Index");
            }

            _context.Comptes.Add(compte);
            await _context.SaveChangesAsync();

            client.CompteId = compte.CompteId; 

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "client", new { client.ClientId });
        }
        [HttpGet]
        public  IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Compte compte)
        {
            if (compte == null)
            {
                return NotFound();
            }
            Compte CompteEnBase = await _context.Comptes.FirstOrDefaultAsync(c => c.Password == compte.Password && c.Email == compte.Email);
            Compte CompteAvecMauvaisMotDePasse = await _context.Comptes.FirstOrDefaultAsync(c => c.Email == compte.Email && c.Password != compte.Password);

        
            if (CompteAvecMauvaisMotDePasse != null)
            {
                TempData["Error"] = "Mot de passe incorrect";
                return RedirectToAction("Login");
            }
            if (CompteEnBase == null)
            {
                TempData["Error"] = "Ce Compte n'existe pas";
                return RedirectToAction("Login");
            }
            Client client = await _context.Clients.FirstOrDefaultAsync(c => c.CompteId == CompteEnBase.CompteId);
            return RedirectToAction("Index", "client", new { client.ClientId });
        }
    }
   
}
