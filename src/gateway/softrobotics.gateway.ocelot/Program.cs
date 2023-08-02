using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using softrobotics.shared.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
               .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration.GetSection("ElasticsearchUrl").Get<string>()!))
               {
                   AutoRegisterTemplate = true,
               })
               .Enrich.FromLogContext()
               .CreateLogger();

builder.Configuration.AddJsonFile("ocelot.json", false, false);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
JwtSettings jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;

builder.Services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.ASCII.GetBytes(jwtSettings.SecretKey)))),
                        ValidIssuer = "http://localhost:26147",
                        ValidateIssuer = true,
                        ValidateAudience = false
                    };
                });

builder.Services.AddOcelot();

var app = builder.Build();

app.UseSerilog();

app.MapGet("/", () => "Hello World!");

app.UseAuthentication();
app.UseAuthorization();
await app.UseOcelot();

app.Run();
