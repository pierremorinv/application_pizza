using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;

namespace WebApplication2.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MenuController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View();
        }
    }
}
