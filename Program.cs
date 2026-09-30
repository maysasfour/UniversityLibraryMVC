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

       
        var adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL") ?? "admin@meu-library.com";
        var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? "Admin@12345";

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

            if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
                await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        await SeedUniversityBranding(context);
        await RemoveLegacyDemoData(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "❌ An error occurred while seeding the database.");
    }
}

app.Run();


async Task SeedUniversityBranding(LibraryDbContext context)
{
    if (!context.Universities.Any())
    {
        context.Universities.Add(new University
        {
            Name = "Middle East University",
            PrimaryColor = "#31443B",
            SecondaryColor = "#9B7A4A",
            LogoPath = "/Images/meu-university-logo.png",
            MenuPosition = 1
        });
        await context.SaveChangesAsync();
    }

    var menuUniversity = context.Universities
        .OrderBy(u => u.MenuPosition)
        .FirstOrDefault();

    if (menuUniversity != null)
    {
        var changed = false;

        if (menuUniversity.PrimaryColor == "#003366" || menuUniversity.PrimaryColor == "#1F5F5B" || menuUniversity.PrimaryColor == "#4A2433")
        {
            menuUniversity.PrimaryColor = "#31443B";
            changed = true;
        }

        if (menuUniversity.SecondaryColor == "#FFD700" || menuUniversity.SecondaryColor == "#D6A756" || menuUniversity.SecondaryColor == "#B8955A")
        {
            menuUniversity.SecondaryColor = "#9B7A4A";
            changed = true;
        }

        if (menuUniversity.Name == "Meu University")
        {
            menuUniversity.Name = "Middle East University";
            changed = true;
        }

        if (menuUniversity.LogoPath == "/images/meu-university-logo.png")
        {
            menuUniversity.LogoPath = "/Images/meu-university-logo.png";
            changed = true;
        }

        if (changed)
            await context.SaveChangesAsync();
    }
}

async Task RemoveLegacyDemoData(LibraryDbContext context)
{
    var demoIsbns = new[]
    {
        "978-0262033848",
        "978-0132350884",
        "978-0135957059",
        "978-1-56-619909-4"
    };

    var demoEmails = new[]
    {
        "moh@yahoo.com",
        "lilly@gmail.com"
    };

    var demoBookIds = context.Books
        .Where(book => demoIsbns.Contains(book.ISBN))
        .Select(book => book.BookID)
        .ToList();

    var demoMemberIds = context.Members
        .Where(member => demoEmails.Contains(member.Email))
        .Select(member => member.MemberID)
        .ToList();

    if (demoBookIds.Count == 0 && demoMemberIds.Count == 0)
        return;

    var demoLoans = context.Loans
        .Where(loan => demoBookIds.Contains(loan.BookID) || demoMemberIds.Contains(loan.MemberID));
    context.Loans.RemoveRange(demoLoans);

    var demoBooks = context.Books.Where(book => demoBookIds.Contains(book.BookID));
    context.Books.RemoveRange(demoBooks);

    var demoMembers = context.Members.Where(member => demoMemberIds.Contains(member.MemberID));
    context.Members.RemoveRange(demoMembers);

    await context.SaveChangesAsync();
}
