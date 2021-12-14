using Microsoft.AspNetCore.Identity;
using Mvc.ApplicationCore.Entities;
using Mvc.ApplicationCore.Identity;
using Mvc.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApp.Infrastructure.Data
{



    public static class ApplicationContextSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole("admin"));
            await roleManager.CreateAsync(new IdentityRole("user"));

            //await dbContext.SaveChangesAsync();
        }

        public static async Task SeedDatabaseAsync(ApplicationContext dbContext)
        {
            if (dbContext.Tags.Any()) return;            

            dbContext.Tags.AddRange(new Tag[]
            {
                new Tag("Sport"),
                new Tag("Films"),
                new Tag("Ecology"),
                new Tag("Space"),
                new Tag("Electronics"),
                new Tag("Games"),
                new Tag("Design"),
                new Tag("Other"),
                new Tag("Mashines"),
                new Tag("Deep dive"),
                new Tag("Health"),
            });
                   
            await dbContext.SaveChangesAsync();
        }
    }
}
