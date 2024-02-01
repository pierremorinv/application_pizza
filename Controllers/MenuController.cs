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
        public async Task <IActionResult> Index()
        {
            PizzaCommandeViewModel pizzaCommandeViewModel = new PizzaCommandeViewModel();

            pizzaCommandeViewModel.Pizzas = await _context.Pizzas.Include(p => p.Ingredients).AsNoTracking().ToListAsync();

            pizzaCommandeViewModel.Commandes = await _context.Commandes.Include(c => c.ligneDeCommandes).AsNoTracking().ToListAsync();

            pizzaCommandeViewModel.LigneDeCommandes = await _context.LigneDeCommandes.Include(lc => lc.Pizza).AsNoTracking().ToListAsync();



            return View(pizzaCommandeViewModel);
        }
    }
}
