using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.Design;
using System.Linq;
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

            pizzaCommandeViewModel.Commande = await _context.Commandes
                .Include(c => c.ligneDeCommandes).ThenInclude(lc => lc.Pizza)
                .Include(c => c.ligneDeCommandes).ThenInclude(lc => lc.Ingredients)
                .AsNoTracking().OrderBy(c => c.ClientID).LastOrDefaultAsync();
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

            Commande? commande = await _context.Commandes
                .Include(c => c.ligneDeCommandes).ThenInclude(lc => lc.Pizza)
                .Include(c => c.ligneDeCommandes).ThenInclude(lc => lc.Ingredients)
                .AsNoTracking().FirstOrDefaultAsync(c => c.CommandeId == CommandeId);

            Pizza? pizza = await _context.Pizzas.Include(p => p.Ingredients).SingleOrDefaultAsync(p => p.PizzaId == PizzaId);
            LigneDeCommande? ligneDeCommande = await _context.LigneDeCommandes
                .Include(lc => lc.Pizza).ThenInclude(p => p.Ingredients)
                .Include(lc => lc.Ingredients)
                .SingleOrDefaultAsync(p => p.PizzaId == PizzaId);

         

            if (pizza == null )
            {
                return NotFound();
            }
            LigneDeCommande NouvelleLigneDeCommande = new LigneDeCommande()
            {
                CommandeId = CommandeId,
                Pizza = pizza,
                QuantitePizza = 1,
                PrixUnitaire = pizza.Prix


            };


            if (commande.ligneDeCommandes.Any(lc => lc.Pizza.PizzaId == PizzaId))

            {
                ligneDeCommande.QuantitePizza++;
                _context.Update(ligneDeCommande);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", new { ligneDeCommande.LigneDeCommandeId });
              
            }
            else 
            {

                commande.ligneDeCommandes.Add(NouvelleLigneDeCommande);
                _context.Update(NouvelleLigneDeCommande);


                await _context.SaveChangesAsync();

                return RedirectToAction("Create", new { NouvelleLigneDeCommande.LigneDeCommandeId });

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


            ligneDeCommande.Ingredients.Add(ingredient);
            ligneDeCommande.PrixUnitaire += ingredient.Prix;

            _context.Update(ingredient);
            await _context.SaveChangesAsync();


            return RedirectToAction("Create", new { LigneDeCommandeId });
        }

        public async Task<IActionResult> ConfirmLigneDeCommande(int LigneDeCommandeId)
        {
            LigneDeCommande? ligneDeCommande = await _context.LigneDeCommandes
                .Include(lc => lc.Ingredients).SingleOrDefaultAsync(lc => lc.LigneDeCommandeId == LigneDeCommandeId);


            return RedirectToAction("Index", new { LigneDeCommandeId });
        }







        public async Task<IActionResult> DeleteExtraIngredientInLigneDeCommande(int IngredientId, int LigneDeCommandeId)
        {

            LigneDeCommande? ligneDeCommande = await _context.LigneDeCommandes
                .Include(lc => lc.Ingredients).ThenInclude(i => i.LigneDeCommandes)
                .FirstOrDefaultAsync(lc => lc.LigneDeCommandeId == LigneDeCommandeId);

            Ingredient ingredient = ligneDeCommande.Ingredients.FirstOrDefault(i => i.IngredientId == IngredientId);

            if (ligneDeCommande == null)
            {
                return NotFound();
            }

            ligneDeCommande.Ingredients.Remove(ingredient);

            _context.Update(ligneDeCommande);
            await _context.SaveChangesAsync();


            return RedirectToAction("Create", new { ligneDeCommande.LigneDeCommandeId });
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

                return RedirectToAction("Index");
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

                return RedirectToAction("Index");
            }
            else
            {
                return (NotFound());
            }
        }

        public async Task<IActionResult> DeleteLigneDeCommandeInCommande(int LigneDeCommandeId)
        {
            LigneDeCommande? ligneDeCommande = await _context.LigneDeCommandes.Include(lc => lc.Ingredients).AsNoTracking().FirstOrDefaultAsync(p => p.LigneDeCommandeId == LigneDeCommandeId);


            if (ligneDeCommande != null)
            {
                _context.LigneDeCommandes.Remove(ligneDeCommande);
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
