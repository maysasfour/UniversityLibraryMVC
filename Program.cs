using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using UniversityLibraryMVC.Models;
using UniversityLibraryMVC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddControllersWithViews();

var dataDirectory = Environment.GetEnvironmentVariable("DATA_DIRECTORY")
    ?? Path.Combine(builder.Environment.ContentRootPath, ".aspnet");
Directory.CreateDirectory(dataDirectory);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(dataDirectory, "DataProtection-Keys")));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("DefaultConnection is not configured.");
var useSqlite = string.Equals(
    Environment.GetEnvironmentVariable("DATABASE_PROVIDER"),
    "Sqlite",
    StringComparison.OrdinalIgnoreCase);

builder.Services.AddDbContext<LibraryDbContext>(options =>
{
    if (useSqlite)
        options.UseSqlite(connectionString);
    else
        options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());
});

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<LibraryDbContext>();

builder.Services.AddScoped<UniversityService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<XmlService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<LibraryDbContext>();

   
        if (useSqlite)
            context.Database.EnsureCreated();
        else
            context.Database.Migrate();

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

      
        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new IdentityRole("Admin"));

        if (!await roleManager.RoleExistsAsync("Member"))
            await roleManager.CreateAsync(new IdentityRole("Member"));

       
        var adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
        var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

        if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
        {
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "User"
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine("Admin user created successfully");
                }
            }
        }

        await SeedSampleData(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "❌ An error occurred while seeding the database.");
    }
}

app.Run();


async Task SeedSampleData(LibraryDbContext context)
{
    
    if (!context.Universities.Any())
    {
        context.Universities.Add(new University
        {
            Name = "Meu University",
            PrimaryColor = "#003366",
            SecondaryColor = "#FFD700",
            LogoPath = "/images/meu-university-logo.png",
            MenuPosition = 1
        });
        await context.SaveChangesAsync();
    }

    var universityId = context.Universities
        .OrderBy(u => u.UniversityID)
        .Select(u => u.UniversityID)
        .First();

    if (!context.Books.Any())
    {
        context.Books.AddRange(
            new Book { Title = "Introduction to Algorithms", Author = "Cormen", ISBN = "978-0262033848", Category = "Computer Science", Publisher = "MIT Press", PublicationYear = 2009, TotalCopies = 5, AvailableCopies = 5, UniversityID = universityId },
            new Book { Title = "Clean Code", Author = "Robert Martin", ISBN = "978-0132350884", Category = "Programming", Publisher = "Prentice Hall", PublicationYear = 2008, TotalCopies = 3, AvailableCopies = 3, UniversityID = universityId },
            new Book { Title = "The Pragmatic Programmer", Author = "Andrew Hunt", ISBN = "978-0135957059", Category = "Programming", Publisher = "Addison-Wesley", PublicationYear = 2019, TotalCopies = 2, AvailableCopies = 2, UniversityID = universityId }
        );
        await context.SaveChangesAsync();
    }

    if (!context.Members.Any())
    {
        context.Members.AddRange(
            new Member { FirstName = "mohammed", LastName = "ahmed", Email = "moh@yahoo.com", Phone = "123-456-7890", Address = "123 Main St", Age = 25, MembershipType = "Student", RegistrationDate = DateTime.Now, MembershipDate = DateTime.Now, StudentID = "STU001", IsActive = true, UniversityID = universityId },
            new Member { FirstName = "lilly", LastName = "hailey", Email = "lilly@gmail.com", Phone = "098-765-4321", Address = "456 Oak Ave", Age = 22, MembershipType = "Student", RegistrationDate = DateTime.Now, MembershipDate = DateTime.Now, StudentID = "STU002", IsActive = true, UniversityID = universityId }
        );
        await context.SaveChangesAsync();
    }

  
    if (!context.Loans.Any())
    {
        var book1 = context.Books.First();
        var member1 = context.Members.First();

        context.Loans.Add(new Loan
        {
            BookID = book1.BookID,
            MemberID = member1.MemberID,
            LoanDate = DateTime.Now.AddDays(-7),
            DueDate = DateTime.Now.AddDays(7),
            ReturnDate = null,
            Status = "Active",
            FineAmount = 0.0m
        });

        await context.SaveChangesAsync();
    }
}
