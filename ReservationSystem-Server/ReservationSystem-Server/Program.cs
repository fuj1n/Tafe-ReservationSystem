using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Services;
using ReservationSystem_Server.Utility;

string[] stringsToTry =
{
    "DefaultConnectionExpress",
    "DefaultConnection",
};

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string? connectionString =
    DatabaseFinder.GetFirstAvailable(stringsToTry.Select(builder.Configuration.GetConnectionString));

if (connectionString == null)
    throw new InvalidOperationException("Could not find the right connection string.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddScoped<CustomerManager>();
builder.Services.AddScoped<ReservationUtility>();

// builder.Services.AddScoped<IActionContextAccessor, ActionContextAccessor>();
// builder.Services.AddScoped<IUrlHelper>(x =>
// {
//         ActionContext?  actionContext = x.GetService<IActionContextAccessor>()?.ActionContext;
//         return new UrlHelper(actionContext ?? throw new InvalidOperationException());
// });

WebApplication app = builder.Build();

// On error, show an error page rather than using the browser one
app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    "area",
    "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();