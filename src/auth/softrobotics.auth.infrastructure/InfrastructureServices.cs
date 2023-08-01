using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using softrobotics.auth.application;
using softrobotics.auth.application.Common.Interface;
using softrobotics.auth.infrastructure.Persistance;

namespace softrobotics.auth.infrastructure;

public static class InfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SoftRoboticsDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("SoftRoboticsDbConnection")));
        services.AddScoped<ISoftRoboticsDbContext>(x => x.GetRequiredService<SoftRoboticsDbContext>());

        services.AddApplicationServices(configuration);
        return services;
    }
}