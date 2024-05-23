using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeBlogProject.Models;
using RecipeProject.Models.DataModels;
using System;
using System.Diagnostics;

namespace TestProject.Controllers.HomeControllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;

        public HomeController(ILogger<HomeController> logger, ModelContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var personId = HttpContext.Session.GetInt32("PersonId");
            TempData["PersonId"] = personId;

            ViewBag.categories = _context.Categories.ToList();

            return View(new Person());
        }

        public async Task<IActionResult> GetRecipesByCategory(int Id)
        {
            var recipesInCategory = await _context.Recipecategories.Where(X => X.CategoryId == Id)
                .Select(Y => Y.Recipe).ToListAsync();
                          
            return View(recipesInCategory);
        }

        public async Task<IActionResult> CheckUserCardInfo(Visacard visacard)
        {
            var id = HttpContext.Session.GetInt32("UserId");
            var checkVisaCardInfo = await _context.Visacards.
                Where(X => X.Cardnumber == visacard.Cardnumber).FirstOrDefaultAsync();

            if (checkVisaCardInfo == null)
                return RedirectToAction("CreateVisaCard");
            else
                return RedirectToAction("UserVisaInformation");
        }

        public async Task<IActionResult> CreateVisaCard(visacardrecipes model)
        {
            var getrecipe = _context.Recipes.Where(X => X.id == model.Recipes.id).Select(Y => Y.id).ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVisaCard(Visacard visacard)
        {
            var id = HttpContext.Session.GetInt32("UserId");
            var checkVisaCardInfo =  await _context.Visacards.
                Where(X => X.Cardnumber == visacard.Cardnumber).FirstOrDefaultAsync();

            if (checkVisaCardInfo == null)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(visacard);
                    await _context.SaveChangesAsync();
                }
            }
            


            return RedirectToAction("UserVisaInformation");
        }

        public  IActionResult UserVisaInformation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserVisaInformation(Visacard visacard)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var visaCardInfo = await _context.Visacards.FirstOrDefaultAsync(u => u.UserId == userId);
            
            visaCardInfo.Firstname = visacard.Firstname;
            visaCardInfo.Lastname = visacard.Lastname;
            visaCardInfo.Cardnumber = visacard.Cardnumber;
            visaCardInfo.Pin = visacard.Pin;
            visaCardInfo.Cvv = visacard.Cvv;
            visaCardInfo.Expirydate = visacard.Expirydate;

            await _context.SaveChangesAsync();

            return View(visacard);
        }

        public async Task<IActionResult> PurchasedRecipes()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var paymentDetails = await _context.Recipepayments.Where(X => X.UserId == userId ).ToListAsync();

          return View(paymentDetails);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
