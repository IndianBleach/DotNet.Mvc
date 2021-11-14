using Mvc.ApplicationCore.Entities;
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
        public static async Task SeedDatabaseAsync(ApplicationContext dbContext)
        {
            if (dbContext.Tags.Any()) return;            

            dbContext.Tags.Add(new Tag("Sport"));

            await dbContext.SaveChangesAsync();
        }
    }
}
