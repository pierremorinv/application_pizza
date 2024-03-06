using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Claims;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.ViewModel;

namespace WebApplication2.Controllers
{
    [Authorize]
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
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            pizzaCommandeViewModel.Commande = await _context.Commandes
                .Include(c => c.Client).Where(c => c.ClientID == userId)
                .Include(c => c.ligneDeCommandes).ThenInclude(lc => lc.Pizza)
                .Include(c => c.ligneDeCommandes).ThenInclude(lc => lc.Ingredients)
                .AsNoTracking().OrderBy(co => co.CommandeId).LastOrDefaultAsync();

            pizzaCommandeViewModel.Pizzas = await _context.Pizzas.Include(p => p.Ingredients).AsNoTracking().ToListAsync();
            pizzaCommandeViewModel.ingredients = await _context.Ingredients.ToListAsync();


            return View(pizzaCommandeViewModel);
        }

        public async Task<IActionResult> Create(int? ligneDeCommandeId)
        {
            OptionViewModel ligneDeCommandeIngredient = new OptionViewModel();

            ligneDeCommandeIngredient.ingredients = await _context.Ingredients.ToListAsync();

            ligneDeCommandeIngredient.LigneDeCommande = await _context.LigneDeCommandes
            .Include(lc => lc.Ingredients)
            .Include(lc => lc.Pizza).ThenInclude(p => p.Ingredients).FirstOrDefaultAsync(lc => lc.LigneDeCommandeId == ligneDeCommandeId);


            if ((ligneDeCommandeId == null) && (ligneDeCommandeIngredient.LigneDeCommande.Pizza != null))
            {
                return NotFound();
            }

            return View(ligneDeCommandeIngredient);
        }

        public async Task<IActionResult> CreateLigneDeCommande(int PizzaId, int CommandeId)
        {
            PizzaCommandeViewModel pizzaCommandeViewModel = new PizzaCommandeViewModel();

            pizzaCommandeViewModel.Commande = await _context.Commandes
                .Include(c => c.Client)
                .Include(c => c.ligneDeCommandes).ThenInclude(lc => lc.Pizza).ThenInclude(lc => lc.Ingredients)
                .Include(c => c.ligneDeCommandes).ThenInclude(lc => lc.Ingredients)
                .FirstOrDefaultAsync(c => c.CommandeId == CommandeId);

            Commande? commande = await _context.Commandes
                .Include(c => c.ligneDeCommandes).ThenInclude(lc => lc.Pizza)
                .Include(c => c.ligneDeCommandes).ThenInclude(lc => lc.Ingredients)
                .FirstOrDefaultAsync(c => c.CommandeId == CommandeId);

            Pizza? pizza = await _context.Pizzas.Include(p => p.Ingredients).SingleOrDefaultAsync(p => p.PizzaId == PizzaId);


            if (pizza == null && commande == null)
            {
                return NotFound();
            }
            LigneDeCommande NouvelleLigneDeCommande = new LigneDeCommande()
            {
                CommandeId = CommandeId,
                Pizza = pizza,
                QuantitePizza = 1,
                PrixUnitaire = pizza.Prix,
                Vegetarien = pizza.Vegetarienne

            };
            if(NouvelleLigneDeCommande.CommandeId == commande.CommandeId)
            {
                commande.PrixTotal += NouvelleLigneDeCommande.PrixUnitaire;
                commande.ligneDeCommandes.Add(NouvelleLigneDeCommande);
                _context.Update(NouvelleLigneDeCommande);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", new { NouvelleLigneDeCommande.LigneDeCommandeId });
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> AddExtraIngredientInLigneDeCommande(int IngredientId, int LigneDeCommandeId)
        {

            LigneDeCommande? ligneDeCommande = await _context.LigneDeCommandes
                .Include(lc => lc.Ingredients).SingleOrDefaultAsync(lc => lc.LigneDeCommandeId == LigneDeCommandeId);

            Ingredient? ingredient = await _context.Ingredients.FindAsync(IngredientId);


            if (ligneDeCommande == null || ingredient == null)
            {
                return NotFound();

            }
            if (!ingredient.Vegetarien)
            {
                ligneDeCommande.Vegetarien = false;
            }
            else
            {
                ligneDeCommande.Vegetarien = true;
            }

            ligneDeCommande.Ingredients.Add(ingredient);
            ligneDeCommande.PrixUnitaire += ingredient.Prix;

            _context.Update(ingredient);
            await _context.SaveChangesAsync();


            return RedirectToAction("Create", new { LigneDeCommandeId });
        }

        public async Task<IActionResult> ConfirmLigneDeCommande(int LigneDeCommandeId)
        {
            // Ligne de commande actuelle
            LigneDeCommande? ligneDeCommande = await _context.LigneDeCommandes
                .Include(lc => lc.Ingredients)
                .Include(lc => lc.Pizza).
                SingleOrDefaultAsync(lc => lc.LigneDeCommandeId == LigneDeCommandeId);

            if (ligneDeCommande == null)
            {
                return NotFound();
            }

            IList<LigneDeCommande>? ligneDeCommandeExistantes = await _context.LigneDeCommandes
             .Include(lc => lc.Ingredients)
             .Include(lc => lc.Pizza)
             .Where(lc => lc.CommandeId == ligneDeCommande.CommandeId && lc.LigneDeCommandeId != ligneDeCommande.LigneDeCommandeId && lc.PizzaId == ligneDeCommande.PizzaId)
             .ToListAsync();

            LigneDeCommande ligneDeCommandeExistante = ligneDeCommandeExistantes.Where(l => l.Ingredients.SequenceEqual(ligneDeCommande.Ingredients)).FirstOrDefault();

            if (ligneDeCommandeExistante != null)
            {
                ligneDeCommandeExistante.QuantitePizza++;
                _context.Remove(ligneDeCommande);
                await _context.SaveChangesAsync();
            }


            return RedirectToAction("Index", new { LigneDeCommandeId });
        }

        public async Task<IActionResult> DeleteExtraIngredientInLigneDeCommande(int IngredientId, int LigneDeCommandeId)
        {

            LigneDeCommande? ligneDeCommande = await _context.LigneDeCommandes
                .Include(lc => lc.Ingredients).ThenInclude(i => i.LigneDeCommandes)
                .FirstOrDefaultAsync(lc => lc.LigneDeCommandeId == LigneDeCommandeId);

            Ingredient ingredient = ligneDeCommande.Ingredients.FirstOrDefault(i => i.IngredientId == IngredientId);

            if (ligneDeCommande == null || ingredient == null)
            {
                return NotFound();
            }
            ligneDeCommande.PrixUnitaire -= ingredient.Prix;
            ligneDeCommande.Ingredients.Remove(ingredient);

            _context.Update(ligneDeCommande);
            await _context.SaveChangesAsync();


            return RedirectToAction("Create", new { ligneDeCommande.LigneDeCommandeId });
        }

        public async Task<IActionResult> AddPizzaQuantity(int PizzaId, int CommandeId, int LigneDeCommandeId)
        {
            
            Pizza? pizza = await _context.Pizzas.AsNoTracking().FirstOrDefaultAsync(p => p.PizzaId == PizzaId);
            Commande? commande = await _context.Commandes
                .Include(c => c.ligneDeCommandes)
                .ThenInclude(cl => cl.Pizza)
                .FirstOrDefaultAsync(c => c.CommandeId == CommandeId);
            LigneDeCommande? ligneDeCommande = await _context.LigneDeCommandes
                .Include(lc => lc.Commande)
                .Include(lc => lc.Pizza)
                .FirstOrDefaultAsync(lc => lc.LigneDeCommandeId == LigneDeCommandeId);


            if (commande.ligneDeCommandes.Contains(ligneDeCommande) && (ligneDeCommande.Pizza.PizzaId == pizza.PizzaId))
            {
                ligneDeCommande.QuantitePizza++;
                _context.Update(ligneDeCommande);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                return (NotFound());
            }
        }

        public async Task<IActionResult> DeletePizzaQuantity(int PizzaId, int CommandeId, int LigneDeCommandeId)
        {
        
            Pizza? pizza = await _context.Pizzas.AsNoTracking().FirstOrDefaultAsync(p => p.PizzaId == PizzaId);
            Commande? commande = await _context.Commandes
                .Include(c => c.ligneDeCommandes)
                .ThenInclude(cl => cl.Pizza)
                .FirstOrDefaultAsync(c => c.CommandeId == CommandeId);

            LigneDeCommande? ligneDeCommande = await _context.LigneDeCommandes
                .Include(lc => lc.Commande)
                .Include(lc => lc.Pizza)
                .FirstOrDefaultAsync(lc => lc.LigneDeCommandeId == LigneDeCommandeId);


            if (commande.ligneDeCommandes.Contains(ligneDeCommande) && (ligneDeCommande.Pizza.PizzaId == pizza.PizzaId) && (ligneDeCommande.QuantitePizza > 1))
            {
                ligneDeCommande.QuantitePizza--;
                _context.Update(ligneDeCommande);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                return (NotFound());
            }
        }

        public async Task<IActionResult> DeleteLigneDeCommandeInCommande(int LigneDeCommandeId, int CommandeId)
        {

            PizzaCommandeViewModel pizzaCommandeViewModel = new PizzaCommandeViewModel();

            LigneDeCommande? LigneDeCommande = await _context.LigneDeCommandes
                .Include(lc => lc.Commande)
                .Include(lc => lc.Pizza).FirstOrDefaultAsync(p => p.LigneDeCommandeId == LigneDeCommandeId);

            Commande? commande = await _context.Commandes.Include(c => c.ligneDeCommandes).ThenInclude(cl => cl.Pizza).FirstOrDefaultAsync(c => c.CommandeId == CommandeId);
            Pizza? pizza = await _context.Pizzas.AsNoTracking().FirstOrDefaultAsync(p => p.PizzaId == LigneDeCommande.Pizza.PizzaId);

            if (commande.ligneDeCommandes.Contains(LigneDeCommande))
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
        public async Task<IActionResult> Validation(int CommandeId, string ClientId)

        {
            PizzaCommandeViewModel pizzaCommandeViewModel = new PizzaCommandeViewModel();
            pizzaCommandeViewModel.Commande = await _context.Commandes
                .Include(c => c.Client)
                .Include(c => c.ligneDeCommandes).ThenInclude(lc => lc.Pizza).ThenInclude(lc => lc.Ingredients)
                .Include(c => c.ligneDeCommandes).ThenInclude(lc => lc.Ingredients)
                .FirstOrDefaultAsync(c => c.CommandeId == CommandeId);

            Client? client = await _context.Clients.FirstOrDefaultAsync(c => c.ClientId == ClientId);

            if (pizzaCommandeViewModel.Commande == null && client.Commandes.Contains(pizzaCommandeViewModel.Commande))
            {
                return (NotFound());
            }
            Commande commande = pizzaCommandeViewModel.Commande;
            commande.CommandeId = CommandeId;
            commande.PrixTotal = 0;

            foreach (var ligne in commande.ligneDeCommandes)
            {
                commande.PrixTotal += ligne.PrixUnitaire;
            }
            _context.Update(commande);
            await _context.SaveChangesAsync();
            return View(pizzaCommandeViewModel.Commande);
        }

    }
}
