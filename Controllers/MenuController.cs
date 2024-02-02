using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.ViewModel;

namespace WebApplication2.Controllers
{
    public class MenuController : Controller
    {

        private readonly ApplicationDbContext _context;
        public MenuController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            PizzaCommandeViewModel pizzaCommandeViewModel = new PizzaCommandeViewModel();
            pizzaCommandeViewModel.Commande = await _context.Commandes.Include(c => c.ligneDeCommandes).ThenInclude(cl => cl.Pizza).AsNoTracking().OrderBy(c => c.ClientID).LastOrDefaultAsync();
            pizzaCommandeViewModel.Pizzas = await _context.Pizzas.Include(p => p.Ingredients).AsNoTracking().ToListAsync();


            return View(pizzaCommandeViewModel);
        }


        public async Task<IActionResult> AddPizzaInLigneDeCommande(int PizzaId, int CommandeId)
        {
            PizzaCommandeViewModel pizzaCommandeViewModel = new PizzaCommandeViewModel();

            Pizza pizza = await _context.Pizzas.AsNoTracking().FirstOrDefaultAsync(p => p.PizzaId == PizzaId);
            Commande commande = await _context.Commandes.Include(c => c.ligneDeCommandes).ThenInclude(cl => cl.Pizza).AsNoTracking().FirstOrDefaultAsync(c => c.CommandeId == CommandeId);


            if ((pizza != null) && (commande != null))
            {
                commande.ligneDeCommandes.Add(new LigneDeCommande
                {
                    CommandeId = commande.CommandeId,
                    Pizza = pizza,
                    PizzaId = pizza.PizzaId,
                    PrixUnitaire = pizza.Prix,
                });
                _context.Update(commande);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { PizzaId, CommandeId });
            }
            else
            {
                return (NotFound());

            }
           

        }

    }
}
