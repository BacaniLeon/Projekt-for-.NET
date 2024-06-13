using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.AddDbContext<GamesDbContext>(options =>
 options.UseSqlServer(
 "Data Source=127.0.0.1;Initial Catalog=Projekt;User ID=sa;Password=yourStrong(!)Password;MultipleActiveResultSets=True;TrustServerCertificate=True;",
 opt => opt.MigrationsAssembly("Vjezba.DAL")));


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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "editgame",
        pattern: "games/edit-game/{id}",
        defaults: new { controller = "Game", action = "Edit" });

    endpoints.MapControllerRoute(
        name: "searchpublishers",
        pattern: "publishers/search-publishers",
        defaults: new { controller = "Publisher", action = "SearchPublishers" });
});


app.Run();
