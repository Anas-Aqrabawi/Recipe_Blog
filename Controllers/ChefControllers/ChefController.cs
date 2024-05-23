using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeBlogProject.Models;

namespace RecipeBlogProject.Controllers.ChefControllers
{


    public class ChefController : Controller
    {
        private readonly ModelContext _context;

        public ChefController(ModelContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var id = HttpContext.Session.GetInt32("ChefID");
            var chef = await _context.Systemusers.Where(X => X.id == id).SingleOrDefaultAsync();

            return View(chef);
        }

        public IActionResult RecipeCategories()
        {
            ViewBag.categories = _context.Categories.ToList();

            return View();
        }

        public async Task<IActionResult> MyRecipes() {

            var recipes = await _context.Recipes.ToListAsync();

            return View(recipes);
        }

        public IActionResult GetChefProfilePage() 
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login_Index", "Login");
        }
    }
}
