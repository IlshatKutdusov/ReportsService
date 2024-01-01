using Application.Common.Interfaces;
using Infrastructure.Files;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddTransient<IFileHelper, FileHelper>();
            services.AddTransient<IReportBuilder, ReportBuilder>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddSingleton(new ApplicationUser());
            return services;
        }
    }
}
