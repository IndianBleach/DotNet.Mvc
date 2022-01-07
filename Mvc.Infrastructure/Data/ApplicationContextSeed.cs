using Microsoft.AspNetCore.Identity;
using Mvc.ApplicationCore.Entities;
using Mvc.ApplicationCore.Entities.IdeaEntity;
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

            dbContext.IdeaStatuses.AddRange(new IdeaStatus[]
            {
                new IdeaStatus(IdeaStatuses.Complete, "The idea is done, wait for the next news and updates"),
                new IdeaStatus(IdeaStatuses.InDevelopment, "The idea is in development, create and fulfill goals"),
                new IdeaStatus(IdeaStatuses.FindMembers, "The idea is looking for participants, tell your friends about it"),
            });

            dbContext.IdeaAvatars.Add(new IdeaAvatarImage("DEFAULT_IDEA_AVATAR.jpg"));

            dbContext.UserAvatars.Add(new UserAvatarImage("DEFAULT_USER_AVATAR.jpg"));

            dbContext.GoalStatuses.AddRange(new BoxGoalStatus[]
            {
                new BoxGoalStatus("Failed", BoxGoalStatuses.Failed),
                new BoxGoalStatus("Complete", BoxGoalStatuses.Complete),
                new BoxGoalStatus("Waiting", BoxGoalStatuses.Waiting),
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
