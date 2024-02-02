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

            pizzaCommandeViewModel.Commande = new Commande() { ClientID = 16,DateCommande = DateTime.Now,ligneDeCommandes = new List<LigneDeCommande>()}; /*await _context.Commandes.Include(c => c.ligneDeCommandes).ThenInclude(lc => lc.Pizza).AsNoTracking().ToListAsync();
*/


            return View(pizzaCommandeViewModel);
        }

        public async Task<IActionResult> AddPizzaInLigneDeCommande(int PizzaId)
        {
            
            LigneDeCommande ligneDeCommande = new LigneDeCommande();
            PizzaCommandeViewModel pizzaCommandeViewModel = new PizzaCommandeViewModel();
            Pizza pizza  = await _context.Pizzas.Include(p => p.Ingredients).FirstOrDefaultAsync(p => p.PizzaId == PizzaId);

            //if (pizza.PizzaId != PizzaId)
            //{
            //    return(NotFound());
            //}
            //else
            //{
             
            //}
            ligneDeCommande.Pizza = pizza;
            ligneDeCommande.PizzaId = PizzaId;
           
            _context.Update(ligneDeCommande);
            await _context.SaveChangesAsync();



            return View(PizzaId);

        }
    }
}
