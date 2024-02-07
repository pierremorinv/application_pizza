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
            pizzaCommandeViewModel.ingredients = await _context.Ingredients.ToListAsync();
            pizzaCommandeViewModel.Option = await _context.Options.ToListAsync();


            return View(pizzaCommandeViewModel);
        }


        public async Task<IActionResult> AddPizzaInLigneDeCommande(int PizzaId, int CommandeId, int LigneDeCommandeId)
        {
            PizzaCommandeViewModel pizzaCommandeViewModel = new PizzaCommandeViewModel();

            Pizza pizza = await _context.Pizzas.AsNoTracking().FirstOrDefaultAsync(p => p.PizzaId == PizzaId);
            Commande commande = await _context.Commandes.Include(c => c.ligneDeCommandes).ThenInclude(cl => cl.Pizza).AsNoTracking().FirstOrDefaultAsync(c => c.CommandeId == CommandeId);
            LigneDeCommande ligneDeCommande = await _context.LigneDeCommandes.FirstOrDefaultAsync(lc => lc.LigneDeCommandeId == LigneDeCommandeId);

            if ((pizza != null) && (commande != null) && !commande.ligneDeCommandes.Contains(ligneDeCommande))
            {
                
                commande.ligneDeCommandes.Add(new LigneDeCommande
                {
                    CommandeId = commande.CommandeId,
                    Pizza = pizza,
                    PizzaId = pizza.PizzaId,
                    PrixUnitaire = pizza.Prix,
                    QuantitePizza = 1                     
                });

                _context.Update(commande);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { PizzaId, CommandeId, LigneDeCommandeId });
            }
            else
            {
                return (NotFound());

            }
        }
        public async Task<IActionResult> AddPizzaQuantity(int PizzaId, int CommandeId, int LigneDeCommandeId)
        {
            PizzaCommandeViewModel pizzaCommandeViewModel = new PizzaCommandeViewModel();
            Pizza pizza = await _context.Pizzas.AsNoTracking().FirstOrDefaultAsync(p => p.PizzaId == PizzaId);
            Commande commande = await _context.Commandes.Include(c => c.ligneDeCommandes).ThenInclude(cl => cl.Pizza).AsNoTracking().FirstOrDefaultAsync(c => c.CommandeId == CommandeId);
            LigneDeCommande ligneDeCommande = await _context.LigneDeCommandes.FirstOrDefaultAsync(lc => lc.LigneDeCommandeId == LigneDeCommandeId);


            if ((pizza != null) && (ligneDeCommande != null))
            {
                ligneDeCommande.QuantitePizza++;
                _context.Update(ligneDeCommande);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { PizzaId, CommandeId, LigneDeCommandeId});
            }
            else
            {
                return (NotFound());
            }
        }
        public async Task<IActionResult> DeletePizzaQuantity(int PizzaId, int CommandeId, int LigneDeCommandeId)
        {
            PizzaCommandeViewModel pizzaCommandeViewModel = new PizzaCommandeViewModel();
            Pizza pizza = await _context.Pizzas.AsNoTracking().FirstOrDefaultAsync(p => p.PizzaId == PizzaId);

            Commande commande = await _context.Commandes.Include(c => c.ligneDeCommandes).AsNoTracking().FirstOrDefaultAsync(c => c.CommandeId == CommandeId);
            LigneDeCommande ligneDeCommande = await _context.LigneDeCommandes.FirstOrDefaultAsync(lc => lc.LigneDeCommandeId == LigneDeCommandeId);


            if ((pizza != null) && (ligneDeCommande != null) && (ligneDeCommande.QuantitePizza > 1))
            {
                ligneDeCommande.QuantitePizza--;
                _context.Update(ligneDeCommande);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { PizzaId, CommandeId, LigneDeCommandeId });
            }
            else
            {
                return (NotFound());
            }
        }

        public async Task<IActionResult> DeleteLigneDeCommandeInCommande(int LigneDeCommandeId, int CommandeId)
        {
            LigneDeCommande LigneDeCommande = await _context.LigneDeCommandes.AsNoTracking().FirstOrDefaultAsync(p => p.LigneDeCommandeId == LigneDeCommandeId);
            Commande commande = await _context.Commandes.Include(c => c.ligneDeCommandes).ThenInclude(cl => cl.Pizza).AsNoTracking().FirstOrDefaultAsync(c => c.CommandeId == CommandeId);

            if ((LigneDeCommande != null) && (commande != null))
            {
               commande.ligneDeCommandes.Remove(LigneDeCommande);
                _context.Remove(LigneDeCommande);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return (NotFound());

            }
        }
    }
}
