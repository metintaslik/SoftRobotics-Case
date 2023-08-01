using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using softrobotics.auth.application.Common.Interface;
using softrobotics.auth.application.Common.Services;
using Microsoft.Extensions.Configuration;
using softrobotics.auth.application.Common.Mapping;

namespace softrobotics.auth.application;

public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddAutoMapper(typeof(MapConfiguration).Assembly);
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddTransient<ITokenHelper, TokenHelper>();
        return services;
    }
}