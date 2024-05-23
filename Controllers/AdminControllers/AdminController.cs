using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeBlogProject.Common;
using RecipeBlogProject.Models;
using RecipeProject.Models.DataModels;

namespace RecipeBlogProject.Controllers.AdminControllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;

        public AdminController(ModelContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var id = HttpContext.Session.GetInt32("AdminId");
            if(id == null)
            {
                return RedirectToAction("Login_Index","Login");
            }
            var personInformation = await _context.Persons.FirstOrDefaultAsync(c => c.id == id);
            TempData["Name"] = personInformation.Firstname + " " + personInformation.Lastname;
            TempData["Email"] = personInformation.Email;
            var admin = await _context.Admins.Where(X => X.id == id).SingleOrDefaultAsync();

            return View(admin);
        }

        public IActionResult GetAllUsers()
        {
            return RedirectToAction("Index", "AdminAllUsers");
        }


        public async Task<IActionResult> GetChefs()
        {
            var chefs = await _context.Chefs.Include(c=>c.Person).ToListAsync();
            return View(chefs);
        }
        public async Task<IActionResult> Categories()
        {
            var categories = await _context.Categories.IgnoreQueryFilters().ToListAsync();
            return View(categories);
        }
        public async Task<IActionResult> CategoryEdit(int id)
        {
            var category = await _context.Categories.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.id ==id);
            return View(category);
        }
        public async Task<IActionResult> CategoryDetails(int id)
        {
            var category = await _context.Categories.IgnoreQueryFilters().Where(c => c.id == id)
                                  .Select(c=> new CategoryRecepies()
                                  {
                                      Category = c,
                                      Recipes =  c.Recipecategories.Select(c => c.Recipe).ToList()
                                  })
                                  .FirstOrDefaultAsync();
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> CategoryDetails(Category category)
        {
            var categoryData = await _context.Categories.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.id == category.id);
            categoryData.Categoryname = category.Categoryname;
            categoryData.IsDeleted = category.IsDeleted;
            await _context.SaveChangesAsync();
            return Redirect(nameof(Categories));
        }
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {

            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw;
            }
            return RedirectToAction(nameof(Categories));
        }
        public async Task<IActionResult> CategoryDelete(int id)
        {
            await _context.Categories.IgnoreQueryFilters().Where(c => c.id == id).ExecuteUpdateAsync(c => c.SetProperty(c => c.IsDeleted , true));
            return RedirectToAction(nameof(Categories));
        }
        public async Task<IActionResult> CategoryUnDelete(int id)
        {
            await _context.Categories.IgnoreQueryFilters().Where(c => c.id == id).ExecuteUpdateAsync(c => c.SetProperty(c => c.IsDeleted, false));
            return RedirectToAction(nameof(Categories));
        }
        public IActionResult GetAdminCategories()
        {
            return RedirectToAction("Index", "AdminCategories");
        }

        public async Task<IActionResult> AdminRecipes()
        {
            var recepies = await _context.Recipes.IgnoreQueryFilters().ToListAsync();
            return View(recepies);
        }
        
        public async Task<IActionResult> AdminEditRecipes(int? id)
        {
            var recepies = await _context.Recipes.FindAsync(id);
            return View(recepies);
        }
        [HttpPost]
        public async Task<IActionResult> AdminEditRecipePost(Recipe recipe)
        {
            var recepieData = await _context.Recipes.FindAsync(recipe.id);
            recepieData.Ingredients = recipe.Ingredients;
            recepieData.Receipename = recipe.Receipename;
            recepieData.Price = recipe.Price;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminRecipes));
        }
        public async Task<IActionResult> AdminRecipeDetails(int? id)
        {
            var recepies = await _context.Recipes.FindAsync(id);
            return View(recepies);
        }

        public async Task<IActionResult> AdminChefRecepies()
        {
            var recepies = await _context.Recipes.Where(r=> !r.Isapproved).ToListAsync();
            return View(recepies);
        }
        public async Task<IActionResult> AdminUserRecepieRequest()
        {
            var recepies = await _context.Recipepayments.ToListAsync();
            return View(recepies);
        }
        [HttpPost]
        public async Task<IActionResult> AdminUserRecepieRequest(DateTime? from, DateTime? to)
        {
            var recepies = await _context.Recipepayments.IgnoreQueryFilters()
                .Where(r => 
                        (from.HasValue? from.Value <= r.CreatedDate : true)
                        && (to.HasValue ? to.Value >= r.CreatedDate : true)
                        )
                .ToListAsync();

            if (from.HasValue || to.HasValue)
            {
                TempData["From"] = from.HasValue ? from.Value.ToString("yyyy-MM-dd") : null;
                TempData["To"] = to.HasValue ? to.Value.ToString("yyyy-MM-dd") : null;
            }
            else
            {
                TempData["From"] = null;
                TempData["To"] = null;
            }
            return View(recepies);
        }
        public async Task<IActionResult> UnApproveRecepie(int? id)
        {
            var recepie = await _context.Recipes.FindAsync(id);
            if (recepie is not null)
            {
                recepie.Isapproved = false;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AdminRecipes));
        }
        public async Task<IActionResult> ApproveRecepie(int? id)
        {
            var recepie = await _context.Recipes.FindAsync(id);
            if(recepie is not null)
            {
                recepie.Isapproved = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AdminRecipes));
        }
        public async Task<IActionResult> DisableRecepie(int? id)
        {
            var recepie = await _context.Recipes.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.id == id);
            if (recepie is not null)
            {
                recepie.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AdminRecipes));
        }
        public async Task<IActionResult> DeleteChef(int? id)
        {
            var chef = await _context.Chefs.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.id == id);
            chef.IsDeleted = true;
            var person = await _context.Persons.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.id == chef.PersonId);
            person.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetChefs));
        }
        public async Task<IActionResult> EnableChef(int? id)
        {
            var chef = await _context.Chefs.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.id == id);
            chef.IsDeleted = false;
            var person = await _context.Persons.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.id == chef.PersonId);
            person.IsDeleted = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetChefs));
        }
        public async Task<IActionResult> EnableRecepie(int? id)
        {
            var recepie = await _context.Recipes.IgnoreQueryFilters().FirstOrDefaultAsync(x=>x.id ==id);
            if (recepie is not null)
            {
                recepie.IsDeleted = false;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AdminRecipes));
        }
        public async Task<IActionResult> AcceptNewChef(int? id)
        {
            var chef = await _context.Chefs.Include(c=>c.Person).IgnoreQueryFilters().FirstOrDefaultAsync(c=>c.id ==id);
            chef.IsDeleted = false;
            chef.Person.IsDeleted = false;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ChefsRegisteration));
        }
        public async Task<IActionResult> AdminChefDetails(int? id)
        {
            var chef = await _context.Chefs.Include(c=>c.Person).IgnoreQueryFilters().FirstOrDefaultAsync(x=>x.id == id);
            return View(chef);
        }


        public IActionResult GetCharts()
        {
            return View();
        }

        public IActionResult GetAdminProfilePage() 
        { 
            return View();
        }
        public async Task<IActionResult> CreateChef()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateChef([Bind("Firstname,Lastname,Gender,Email,Phone,Password,CreatedDate,ModifiedDate,CreatedBy,ModifiedBy,IsDeleted,id")] Person person)
        {
            if (ModelState.IsValid)
            {
                person.RoleId = (int)Roles.Chef;
                _context.Add(person);
                await _context.SaveChangesAsync();
                var personData = await _context.Persons.FirstAsync(c => c.Email == person.Email);
                var chef = new Chef()
                {
                    PersonId = personData.id
                };
                await _context.Chefs.AddAsync(chef);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GetChefs));
            }
            return View(person);
        }

        public async Task<IActionResult> ChefsRegisteration()
        {
            var newChefs = await _context.Chefs.Include(c => c.Person).Where(c=>c.IsDeleted == true).IgnoreQueryFilters().ToListAsync();
            return View(newChefs);
        }
        public IActionResult Logout()
        {
           var t = _context.Recipepayments.Where(c => c.UserId == 1).Select(c => c.Recipe);
            HttpContext.Session.Clear();
            return RedirectToAction("Login_Index", "Login");
        }
    }
}
