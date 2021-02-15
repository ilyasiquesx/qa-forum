using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QAForum.Application.Common.Interfaces;
using QAForum.Infrastructure.Common.Constants;
using QAForum.Infrastructure.Context;
using QAForum.Infrastructure.Identity;
using QAForum.Infrastructure.Repositories;

namespace QAForum.Infrastructure.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration[ConfigurationSections.ConnectionString];
            services.AddDbContext<AppDbContext>(o => o.UseNpgsql(connectionString));
            services.AddIdentity<AppUser, IdentityRole>(opt =>
                {
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequireDigit = false;
                    opt.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<AppDbContext>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IQuestionRepository, QuestionRepository>();
            services.AddTransient<IAnswerRepository, AnswerRepository>();

            return services;
        }
    }
}