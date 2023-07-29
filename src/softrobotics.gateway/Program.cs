using CacheManager.Core;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using softrobotics.gateway;

var builder = WebApplication.CreateBuilder(args);

#region Variables

CacheOption cacheOptions = builder.Configuration.GetSection("CacheOptions").Get<CacheOption>();

#endregion

#region Configurations

builder.Configuration.AddJsonFile("appsettings.json", true, true)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                     .AddEnvironmentVariables();

#endregion

#region Services

builder.Services.AddOcelot()
                .AddCacheManager(x => x.WithDictionaryHandle())
                .AddCacheManager(x =>
                    x.WithRedisConfiguration(cacheOptions.Type, config =>
                    {
                        config.WithAllowAdmin()
                              .WithDatabase(cacheOptions.DatabaseIndex)
                              .WithEndpoint(cacheOptions.Host, cacheOptions.Port)
                              .WithConnectionTimeout(cacheOptions.ExpirySeconds);
                    })
                    .WithJsonSerializer()
                    .WithRedisCacheHandle(cacheOptions.Type)
                );

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseOcelot().Wait();

app.MapGet("/", () => "Hello Ocelot API Gateway!");

app.Run();