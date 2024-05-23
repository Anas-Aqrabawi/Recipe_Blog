using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Hosting;
using RecipeBlogProject.Common;
using RecipeProject.Common;

namespace RecipeBlogProject.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Chef> Chefs { get; set; }

    public virtual DbSet<Person> Persons { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Recipecategory> Recipecategories { get; set; }

    public virtual DbSet<Recipepayment> Recipepayments { get; set; }

    public virtual DbSet<Systemuser> Systemusers { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<Userrole> Userroles { get; set; }

    public virtual DbSet<Visacard> Visacards { get; set; }

    public virtual DbSet<Websitedetail> Websitedetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            //other automated configurations left out
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                entityType.AddSoftDeleteQueryFilter();
            }
        }
        SeedRoles(modelBuilder);
        SeedPersons(modelBuilder);
        SeedCategories(modelBuilder);
        SeedRecepies(modelBuilder);
        SeedRecepiesCategories(modelBuilder);
        SeedPayment(modelBuilder);
        SeedVisaCard(modelBuilder);
        SeedUserTestmonial(modelBuilder);
        SeedWebisteHomePage(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private void SeedWebisteHomePage(ModelBuilder modelBuilder)
    {
        var data =  new List<Websitedetail>() 
        {
            new Websitedetail()
            {
                id = 1,
                AdminId = 1,
                Texttype = (int)WebsiteContent.HomePageMainParagraph,
                Websitetext = "home page details"
            }
        };
        modelBuilder.Entity<Websitedetail>().HasData(data);
    }

    private void SeedUserTestmonial(ModelBuilder modelBuilder)
    {
        var testmonials  = new List<Testimonial>()
        {
            new Testimonial()
            {
                id = 1,
                UserId = 1,
                IsShown = true,
                Rating = 5,
                Usercomment ="Amazing service , delecious recipes",
            }
        };

        modelBuilder.Entity<Testimonial>().HasData(testmonials);
    }

    private void SeedVisaCard(ModelBuilder modelBuilder)
    {
       var visaCards = new List<Visacard>()
       { 
        new Visacard()
        {
            id = 1,
            Cardnumber = 1234131,
            Cvv=123,
            Firstname = "User",
            Lastname = "User",
            Expirydate = "01/27",
            Pin = 0101,
            UserId = 3,

        }
       };
    }

    private void SeedPayment(ModelBuilder modelBuilder)
    {
        var userPayment = new List<Recipepayment>()
        {
            new Recipepayment()
            {
                id = 1,
                RecipeId = 1,
                UserId = 1,
                Totalamount = 23.5,
                Paymentfilepath = "file path"
            },
            new Recipepayment()
            {
                id = 2,
                RecipeId = 2,
                UserId = 1,
                Totalamount = 13.5,
                Paymentfilepath = "file path2"
            },
        };
        modelBuilder.Entity<Recipepayment>().HasData(userPayment);
    }

    private void SeedRecepiesCategories(ModelBuilder modelBuilder)
    {
        var Recipecategories = new List<Recipecategory>()
        {
            new()
            {
                id = 1,
                RecipeId = 1,
                CategoryId = 1,
            },
            new()
            {
                id = 2,
                RecipeId = 1,
                CategoryId= 4,
            },
            new()
            {
                id = 3,
                RecipeId = 2,
                CategoryId= 3,
            },
            new()
            {
                id = 4,
                RecipeId = 2,
                CategoryId= 2,
            }
        };
        modelBuilder.Entity<Recipecategory>().HasData(Recipecategories);
    }

    private void SeedRecepies(ModelBuilder modelBuilder)
    {
        var recepies = new List<Recipe>()
        {
            new Recipe()
            {
                id = 1,
                ChefId = 1,
                Receipename = "Meat with Salt Recepie",
                Isapproved = true,
                Price = 23.5,
                Ingredients = "meat, salt",
                ImagePath = "fdsfds"
            },
             new Recipe()
            {
                id = 2,
                ChefId = 1,
                Receipename = "Chicken with Salt Recepie",
                Isapproved = true,
                Price = 13.5,
                Ingredients = "chicken, salt",
                ImagePath = "fdfdsfsf"
            },
        };
        modelBuilder.Entity<Recipe>().HasData(recepies);
    }

    private void SeedCategories(ModelBuilder modelBuilder)
    {
        var categories = new List<Category>() 
        {
            new Category()
            {
                id = 1,
                Categoryname = "Breakfast",
            },
            new Category()
            {
                id = 2,
                Categoryname = "Lunch",
            },
            new Category()
            {
                id = 3,
                Categoryname = "Dinner",
            },
            new Category()
            {
                id = 4,
                Categoryname = "Sweets",
            },
            new Category()
            {
                id = 5,
                Categoryname = "Juices",
            },
            new Category()
            {
                id = 6,
                Categoryname = "Vegeterian",
            },
        };
        modelBuilder.Entity<Category>().HasData(categories);
    }

    private void SeedRoles(ModelBuilder modelBuilder)
    {
       var roles = new List<Userrole>() {
           new Userrole()
           {
               id = (int)Roles.Administrator,
               RoleName="Admin"
           },
           new Userrole()
           {
               id = (int)Roles.Chef,
               RoleName="Chef"
           },
           new Userrole()
           {
               id = (int)Roles.RegisteredUser,
               RoleName="User"
           },  
           new Userrole()
           {
               id = (int)Roles.GuestUser,
               RoleName="Guest User"
           },
       };
        modelBuilder.Entity<Userrole>().HasData(roles);
    }

    private static void SeedPersons(ModelBuilder modelBuilder)
    {
        var persons = new List<Person>() {
            new Person()
            {
                id = 1,
                Firstname = "Admin",
                Lastname = "Admin",
                Email="Admin@Admin.com",
                Password="Test@12345",
                Phone="0771",
                Gender="Male",
                RoleId=(int)Roles.Administrator
            },
             new Person()
            {
                id = 2,
                Firstname = "Chef",
                Lastname = "Chef",
                Email="Chef@Chef.com",
                Password="Test@12345",
                Phone="0771",
                Gender="Male",
                RoleId=(int)Roles.Chef
            },
             new Person()
            {
                id = 3,
                Firstname = "User",
                Lastname = "User",
                Email="User@User.com",
                Password="Test@12345",
                Phone="0771",
                Gender="Male",
                RoleId=(int)Roles.RegisteredUser
            },
        };
        modelBuilder.Entity<Person>().HasData(persons);
        modelBuilder.Entity<Admin>().HasData(new List<Admin>() { new Admin() { id = 1, PersonId = 1 } }); 
        modelBuilder.Entity<Chef>().HasData(new List<Chef>() { new Chef() { id = 1, PersonId = 2 } }); 
        modelBuilder.Entity<Systemuser>().HasData(new List<Systemuser>() { new Systemuser() { id = 1, PersonId = 3 } }); 
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).ModifiedDate = DateTime.Now;

            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).ModifiedDate = DateTime.Now;

            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
            }
        }

        return base.SaveChanges();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("USER ID=C##GHADA;PASSWORD=123456;DATA SOURCE=localhost:1521/xe");

   

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
public static class SoftDeleteQueryExtension
{
    public static void AddSoftDeleteQueryFilter(
        this IMutableEntityType entityData)
    {
        var methodToCall = typeof(SoftDeleteQueryExtension)
            .GetMethod(nameof(GetSoftDeleteFilter),
                BindingFlags.NonPublic | BindingFlags.Static)
            .MakeGenericMethod(entityData.ClrType);
        var filter = methodToCall.Invoke(null, new object[] { });
        entityData.SetQueryFilter((LambdaExpression)filter);
        entityData.AddIndex(entityData.
             FindProperty(nameof(BaseEntity.IsDeleted)));
    }

    private static LambdaExpression GetSoftDeleteFilter<TEntity>()
        where TEntity : BaseEntity
    {
        Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
        return filter;
    }
}
