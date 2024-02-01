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
            PizzaLigneDeCommandeViewModel pizzaLigneDeCommandeViewModel = new PizzaLigneDeCommandeViewModel();

            pizzaLigneDeCommandeViewModel.Pizzas = await _context.Pizzas.Include(p => p.Ingredients).AsNoTracking().ToListAsync();

            IList<Pizza> PizzasCommande = new List<Pizza>();

            return View(pizzaLigneDeCommandeViewModel);
        }
    }
}
