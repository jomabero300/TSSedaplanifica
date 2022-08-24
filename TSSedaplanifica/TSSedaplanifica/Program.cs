using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Helpers;
using TSSedaplanifica.Helpers.Gene;
using TSSedaplanifica.Helpers.PDF;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(cfg =>
{
    cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
    cfg.SignIn.RequireConfirmedEmail = true;
    cfg.Password.RequireDigit = true;
    cfg.Password.RequireLowercase = true;
    cfg.Password.RequireNonAlphanumeric = true;
    cfg.Password.RequireUppercase = true;
    cfg.Password.RequiredLength = 6;
    cfg.Password.RequiredUniqueChars = 1;

    cfg.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
    cfg.Lockout.MaxFailedAccessAttempts = 3;
    cfg.Lockout.AllowedForNewUsers = true;

    cfg.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABDCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    cfg.User.RequireUniqueEmail = false;

})
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Home/NotAuthorized";
    options.AccessDeniedPath = "/Home/NotAuthorized";
});

builder.Services.AddTransient<SeedDb>();
builder.Services.AddScoped<ICategoryTypeHelper, CategoryTypeHelper>();
builder.Services.AddScoped<ICategoryHelper, CategoryHelper>();
builder.Services.AddScoped<IMeasureUnitHelper, MeasureUnitHelper>();
builder.Services.AddScoped<IZoneHelper, ZoneHelper>();
builder.Services.AddScoped<ISolicitStateHelper, SolicitStateHelper>();
builder.Services.AddScoped<ISolicitDetailHelper, SolicitDetailHelper>();
builder.Services.AddScoped<ISolicitHelper, SolicitHelper>();
builder.Services.AddScoped<IProductHelper, ProductHelper>();
builder.Services.AddScoped<IProductCategoryHelper, ProductCategoryHelper>();
builder.Services.AddScoped<ICategoryTypeDerHelper, CategoryTypeDerHelper>();
builder.Services.AddScoped<ICityHelper, CityHelper>();
builder.Services.AddScoped<ISchoolHelper, SchoolHelper>();
builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<ISchoolUserHelper, SchoolUserHelper>();
builder.Services.AddScoped<IPdfDocumentHelper, PdfDocumentHelper>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IMailHelper, MailHelper>();

builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStatusCodePagesWithReExecute("/error/{0}");

SeedData(app);

void SeedData(WebApplication app)
{
    IServiceScopeFactory scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (IServiceScope scope = scopedFactory.CreateScope())
    {
        SeedDb service = scope.ServiceProvider.GetService<SeedDb>();
        service.SeedAsync().Wait();
    }
}

// Configure the HTTP request pipeline.
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
