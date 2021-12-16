﻿using Mvc.ApplicationCore.Entities;
using Mvc.ApplicationCore.Interfaces;
using Mvc.Infrastructure.Repositories;
using Mvc.Infrastructure.Services;

namespace Mvc.WebUi.Configuration
{
    public static class ConfigureCoreServices
    {
        public static void AddCoreServices(this IServiceCollection services)
        {

            //services.AddScoped<IRepository<>, Repository<>>();

            services.AddScoped(typeof(IIdeaRepository), typeof(IdeaRepository));



            services.AddScoped<ITagService, TagService>();

            services.AddTransient<IAuthorizationService, AuthorizationService>();
        }
    }
}
