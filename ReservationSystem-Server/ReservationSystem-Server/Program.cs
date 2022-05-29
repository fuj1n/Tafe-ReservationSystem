using System.Net;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using ReservationSystem_Server.Areas.Api.Models;
using ReservationSystem_Server.Configuration;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Helper;
using ReservationSystem_Server.Services;

string[] stringsToTry =
{
    "DefaultConnectionExpress",
    "DefaultConnection",
};

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string? connectionString =
    DatabaseFinder.GetFirstAvailable(stringsToTry.Select(name => (name, builder.Configuration.GetConnectionString(name))));

if (connectionString == null)
    throw new InvalidOperationException("Could not find the right connection string.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Adds both cookie and JWT Bearer token based authentication, so that you can still sign in using the website.
// The policy scheme is used to determine which authentication scheme should be used so that both will work.
builder.Services.AddAuthentication(o =>
        {
            o.DefaultScheme = "JWT_OR_COOKIE";
            o.DefaultChallengeScheme = "JWT_OR_COOKIE";
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,

                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

                    // Prevents tokens without an expiry from ever working, as that would be a security vulnerability.
                    RequireExpirationTime = true,

                    // ClockSkew generally exists to account for potential clock difference between issuer and consumer
                    // But we are both, so we don't need to account for it.
                    // For all intents and purposes, this is optional
                    ClockSkew = TimeSpan.Zero
            };
        })
        .AddPolicyScheme("JWT_OR_COOKIE", null, o =>
        {
            o.ForwardDefaultSelector = c =>
            {
                string auth = c.Request.Headers[HeaderNames.Authorization];
                if (!string.IsNullOrWhiteSpace(auth) && auth.StartsWith("Bearer "))
                {
                    return JwtBearerDefaults.AuthenticationScheme;
                }

                return IdentityConstants.ApplicationScheme;
            };
        });

// Prevent redirects to the login page from API endpoints.
builder.Services.ConfigureApplicationCookie(o =>
{
    o.Events = new CookieAuthenticationEvents
    {
        OnRedirectToAccessDenied = ctx =>
        {
            if (ctx.Request.Path.StartsWithSegments("/api"))
            {
                ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Task.CompletedTask;
            }

            ctx.Response.Redirect(ctx.RedirectUri);
            return Task.CompletedTask;
        },
        OnRedirectToLogin = ctx =>
        {
            if (ctx.Request.Path.StartsWithSegments("/api"))
            {
                ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Task.CompletedTask;
            }

            ctx.Response.Redirect(ctx.RedirectUri);
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
});

builder.Services.AddSwaggerGen(o =>
{
    o.CustomSchemaIds(type => type.ToString().Replace('+', '_'));
    o.OperationFilter<ContentTypeOperationFilter>();

    // Set the comments path for the Swagger JSON and UI.
    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    o.IncludeXmlComments(xmlPath);

    o.MapType<TimeSpan>(() => new OpenApiSchema { Type = "string", Example = new OpenApiString("00:30:00") });
    o.MapType<LinkModel>(() => new OpenApiSchema
    {
        Example = new OpenApiObject
        {
            ["href"] = new OpenApiString("objects/values/1"),
            ["rel"] = new OpenApiString("self"),
            ["method"] = new OpenApiString("GET")
        }
    });
    o.MapType<EmptyResult>(() => new OpenApiSchema { Type = "null", Example = new OpenApiNull() });
});

builder.Services.AddScoped<CustomerManager>();
builder.Services.AddScoped<ReservationUtility>();
builder.Services.AddScoped<SittingUtility>();

// builder.Services.AddScoped<IActionContextAccessor, ActionContextAccessor>();
// builder.Services.AddScoped<IUrlHelper>(x =>
// {
//         ActionContext?  actionContext = x.GetService<IActionContextAccessor>()?.ActionContext;
//         return new UrlHelper(actionContext ?? throw new InvalidOperationException());
// });

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.EnableTryItOutByDefault();
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "Reservation API v1");
    });
}

//TODO: Make CORS a per-controller option that is off by default.
app.UseCors(configure =>
{
    configure.AllowAnyHeader();
    configure.AllowAnyOrigin();
    configure.AllowAnyMethod();
});

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