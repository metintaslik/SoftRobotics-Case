using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using softrobotics.auth.application.Common.Interface;
using softrobotics.auth.application.Common.Services;
using Microsoft.Extensions.Configuration;
using softrobotics.auth.application.Common.Mapping;
using MassTransit;

namespace softrobotics.auth.application;

public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddAutoMapper(typeof(MapConfiguration).Assembly);
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddTransient<ITokenHelper, TokenHelper>();

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(configuration.GetSection("RabbitMqUri").Get<string>()!), config =>
                {
                    config.Username("guest");
                    config.Password("guest");
                });
            });
        });

        return services;
    }
}