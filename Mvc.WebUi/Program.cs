using Microsoft.EntityFrameworkCore;
using Mvc.ApplicationCore.Interfaces;
using Mvc.Infrastructure.Data;
using Mvc.Infrastructure.Services;
using MvcApp.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(builder.Configuration
                .GetConnectionString("DatabaseConnectionPath")));

builder.Services.AddTransient<ITagService, TagService>();




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


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

    await ApplicationContextSeed.SeedDatabaseAsync(context);
}


app.Run();
