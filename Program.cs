using System.Configuration;
using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.Models;
using WebStore.Services;
using WebStore.Services.Implements;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

Helper.Config = builder.Configuration;

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"),
        npgsqlOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null);
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();