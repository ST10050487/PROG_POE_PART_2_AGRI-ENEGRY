using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PROG_POE_PART_2_AGRI_ENEGRY.Areas.Data;
using PROG_POE_PART_2_AGRI_ENEGRY.Data;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configure the ApplicationDbContext for Identity
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Identity with roles and default Identity user
builder.Services.AddDefaultIdentity<PlatformUsers>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()  // Adding support for roles
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Ensure authentication is added to the pipeline
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Create Roles and assign default User to a Role
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<PlatformUsers>>();

    // Ensure the roles are created
    string[] roles = { "Admin", "Employee", "Farmer", "Visitor" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Ensure default admin user is created and assigned Admin role
    string adminEmail = "admin@admin.com";
    string adminPassword = "Admin@1234";
    try
    {
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new PlatformUsers
            {
                UserName = adminEmail,
                Email = adminEmail,
                Name = "Admin",
                Surname = "Admin",
                CellPhoneNumber = "0123456789",
                Address = "123 Admin Street",
                PhoneNumber = "0123456789"
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Employee");
            }
            else
            {
                // Handle creation failure (e.g., log errors)
            }
        }
    }
    catch (Exception ex)
    {
        // Log the exception or handle it appropriately
        Console.WriteLine($"An error occurred while creating the admin user: {ex.Message}");
    }

}

app.Run();
