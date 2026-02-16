using DAL.Context;
using DAL.Repository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LibraryDb;Trusted_Connection=True;MultipleActiveResultSets=true"));

builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Book>, BookRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Ensure database is created and seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<LibraryContext>();
        context.Database.EnsureCreated();

        if (!context.Users.Any())
        {
            var user1 = new User { Name = "John Doe", Email = "john.doe@example.com" };
            var user2 = new User { Name = "Jane Smith", Email = "jane.smith@example.com" };

            context.Users.AddRange(user1, user2);
            context.SaveChanges();

            context.Books.AddRange(
                new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", PublicationYear = 1925, UserId = user1.Id },
                new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", PublicationYear = 1960, UserId = user1.Id },
                new Book { Title = "1984", Author = "George Orwell", PublicationYear = 1949, UserId = user2.Id },
                new Book { Title = "Pride and Prejudice", Author = "Jane Austen", PublicationYear = 1813, UserId = user2.Id }
            );
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

app.Run();
