using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using TestVoorToets.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

var connectionString = builder.Configuration.GetConnectionString("BlogDatabase");
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlite(connectionString));
// Add services to the container.
builder.Services.AddControllersWithViews();

// Voeg de sessie service toe
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Voeg de sessie middleware toe
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
