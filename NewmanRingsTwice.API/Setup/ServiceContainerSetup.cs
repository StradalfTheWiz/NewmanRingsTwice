using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewmanRingsTwice.DB.NewmanDB;
using NewmanRingsTwice.Domain.Contracts.Repository;
using NewmanRingsTwice.Domain.Services;
using NewmanRingsTwice.Domain.Services.Contracts;
using NewmanRingsTwice.Repository.Services;

namespace NewmanRingsTwice.API.Setup
{
    public static class ServiceContainerSetup 
    {
        public static void SetServices(this IServiceCollection services, Domain.Shared.EnvironmentType env, ConfigurationManager configuration)
        {
            services.AddScoped<IEnvironmentService, EnvironmentService>();

            services.AddScoped<IMailService, MailService>();

            // Environment-specific db context.
            var crmConnectionString = configuration.GetConnectionString("NewmanDB");

            services.AddDbContext<NewmanDBContext>(options =>
            {
                options.UseSqlServer(crmConnectionString);
            }
            );

            services.AddScoped<IMailRepository, MailRepository>();
        }

        public static void SetMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
