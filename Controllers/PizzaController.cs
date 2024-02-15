using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.ViewModel;

namespace WebApplication2.Controllers
{
    public class PizzaController : Controller
    {
       
        private readonly ApplicationDbContext _context;

        public PizzaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pizzas
        public async Task<IActionResult> Index()
        {

            return View(await _context.Pizzas.Include(p => p.Ingredients).AsNoTracking().ToListAsync());

        }

        // GET: Pizzas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizza = await _context.Pizzas
                .FirstOrDefaultAsync(m => m.PizzaId == id);
            if (pizza == null)
            {
                return NotFound();
            }

            return View(pizza);
        }

        // GET: Pizzas/Create
        public async Task<IActionResult> Create()
        {
            PizzaViewModel pizzaViewModel = new PizzaViewModel();
            pizzaViewModel.IngredientsDisponible = await _context.Ingredients.ToListAsync();
            return View(pizzaViewModel);
        }

        // POST: Pizzas/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pizza pizza)
        {
            PizzaViewModel pizzaViewModel = new PizzaViewModel();
      
            pizzaViewModel.IngredientsDisponible = await _context.Ingredients.ToListAsync();
            
            

            
            if (ModelState.IsValid)
            {
                //pizza.Ingredients = pizzaViewModel.IngredientsDisponible.ToList();
               
                _context.Add(pizza);

                await _context.SaveChangesAsync();
               
                return RedirectToAction("Edit", new { Id = pizza.PizzaId });
            }
            return View(pizza);
        }

        // GET: Pizzas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            PizzaViewModel pizzaViewModel = new PizzaViewModel();

            List<Ingredient> ingredientsInPizza = new List<Ingredient>();



            pizzaViewModel.Pizza = await _context.Pizzas.Include(p => p.Ingredients).SingleOrDefaultAsync(p => p.PizzaId == id);
            
            ingredientsInPizza = pizzaViewModel.Pizza.Ingredients.ToList();

            pizzaViewModel.IngredientsDisponible = await _context.Ingredients.ToListAsync();

            foreach(var ingredient in  ingredientsInPizza)
            {
                pizzaViewModel.Pizza.Prix += ingredient.Prix;
            }
          


            return View(pizzaViewModel);
        }

        // POST: Pizzas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PizzaId,Nom,Prix,Vegetarienne")] Pizza pizza)
        {
            if (id != pizza.PizzaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pizza);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PizzaExists(pizza.PizzaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pizza);
        }

        public async Task<IActionResult> AddIngredientInPizza(int PizzaId, int IngredientId)
        {
            
            Pizza? pizza = await _context.Pizzas.Include(p => p.Ingredients).SingleOrDefaultAsync(p => p.PizzaId == PizzaId);
            Ingredient? ingredient = await _context.Ingredients.FindAsync(IngredientId);

            
            pizza.Ingredients.Add(ingredient);
            pizza.Prix += ingredient.Prix;

            if (!ingredient.Vegetarien)
            {
                pizza.Vegetarienne = false;
            }

            _context.Update(pizza);
           

            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { Id = PizzaId });

        }
       

        // GET: Pizzas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizza = await _context.Pizzas
                .FirstOrDefaultAsync(m => m.PizzaId == id);
            if (pizza == null)
            {
                return NotFound();
            }

            return View(pizza);
        }

        public async Task<IActionResult> DeleteIngredientInPizza(int PizzaId, int IngredientId)
        {
            Pizza pizza = await _context.Pizzas.Include(p => p.Ingredients).SingleOrDefaultAsync(p => p.PizzaId == PizzaId);

            Ingredient ingredient = await _context.Ingredients.FindAsync(IngredientId);

            pizza.Prix -= ingredient.Prix;


            foreach (var i in pizza.Ingredients)
            {

                if (!i.Vegetarien)
                {
                    pizza.Vegetarienne = false;
                }


                pizza.Vegetarienne = true;
            }

            pizza.Ingredients.Remove(ingredient);
            _context.Update(pizza);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { Id = PizzaId });
        }
        // POST: Pizzas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pizza = await _context.Pizzas.Include(p => p.Ingredients).SingleOrDefaultAsync(p => p.PizzaId == id);
    
            if (pizza != null)
            {
                pizza.Ingredients.Clear();
                _context.Pizzas.Remove(pizza);

            }
       
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PizzaExists(int id)
        {
            return _context.Pizzas.Any(e => e.PizzaId == id);
        }
    }
}
