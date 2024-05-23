using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeBlogProject.Common;
using RecipeBlogProject.Models;

namespace RecipeProject.Controllers.HomeControllers
{
    public class UserProfileController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly INotyfService _notyf;

        public UserProfileController(ModelContext context, IWebHostEnvironment webHostEnvironment, INotyfService notyf)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _notyf = notyf;
        }

        // GET: UserProfile/Edit/5
        public async Task<IActionResult> Edit()
        {
            var id = HttpContext.Session.GetInt32("PersonId");
            var person = await _context.Persons.FindAsync(id);


            if (person == null)
            {
                return NotFound();
            }

            ViewData["RoleId"] = new SelectList(_context.Userroles, "id", "id", person.RoleId);

            return View(person);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Person person)
        {
            var id = HttpContext.Session.GetInt32("PersonId");
            var personInfo = await _context.Persons.FindAsync(id);

            personInfo.Firstname = person.Firstname;
            personInfo.Lastname = person.Lastname;
            personInfo.Gender = person.Gender;
            personInfo.Phone = person.Phone;
            personInfo.Email = person.Email;
            personInfo.Password = person.Password;

            await _context.SaveChangesAsync();
            _notyf.Custom("Successfully Updated", 5, "green", "fa fa-check-square-o");

            return View(person);
        }

        //[HttpPost]
        //public async Task<IActionResult> UploadProfilePicture(Person person)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if(person.ImageFile != null)
        //        {
        //            string wwwRootPath = _webHostEnvironment.WebRootPath;
        //            string imageName = Guid.NewGuid().ToString() + "_" + person.ImageFile.FileName;
        //            string fullPath = Path.Combine(wwwRootPath, "/Images/profile-images/" ,imageName);

        //            using(var filestream = new FileStream(fullPath, FileMode.Create))
        //            {
        //                await person.ImageFile.CopyToAsync(filestream);
        //            }

        //            person.ImagePath = imageName;
        //        }
        //    }

        //    _context.Add(person);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("Edit" , "UserProfile");
        //}

        private bool PersonExists(int id)
        {
          return (_context.Persons?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
