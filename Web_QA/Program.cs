using GestionAnomalies.Data;
using GestionAnomalies.Repositories;
using GestionAnomalies.Repositories.Interfaces;
using GestionAnomalies.Services;
using GestionAnomalies.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Chaîne de connexion 'DefaultConnection' introuvable.");
    
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

// CONFIGURATION AUTHENTIFICATION
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
    });

// INJECTION DES REPOSITORIES
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAnomalieRepository, AnomalieRepository>();
builder.Services.AddScoped<IProjetRepository, ProjetRepository>();
builder.Services.AddScoped<ITypeAnomalieRepository, TypeAnomalieRepository>();
builder.Services.AddScoped<IPrioriteRepository, PrioriteRepository>();
builder.Services.AddScoped<IStatutRepository, StatutRepository>();
builder.Services.AddScoped<ICommentaireRepository, CommentaireRepository>();
builder.Services.AddScoped<IPieceJointeRepository, PieceJointeRepository>();

// INJECTION DES SERVICES
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IAnomalieService, AnomalieService>();
builder.Services.AddScoped<IProjetService, ProjetService>();
builder.Services.AddScoped<IReferentielService, ReferentielService>();
builder.Services.AddScoped<IAutorisationService, AutorisationService>();
builder.Services.AddScoped<ICommentaireService, CommentaireService>();
builder.Services.AddScoped<IPieceJointeService, PieceJointeService>();

var app = builder.Build();

// Seed data at startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        DbSeeder.Seed(context);
        Console.WriteLine("DbSeeder exécuté avec succès !");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur lors de l'exécution du DbSeeder : {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
