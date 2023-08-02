using MassTransit;
using softrobotics.communication.api.Interfaces;
using softrobotics.communication.api.Services;
using softrobotics.communication.api.Worker;
using softrobotics.shared.Common.Enums;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(mt =>
{
    mt.AddBus(bus => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host(builder.Configuration.GetSection("RabbitMqUri").Get<string>(), rabbitConfig =>
        {
            rabbitConfig.Username("guest");
            rabbitConfig.Password("guest");
        });

        cfg.ReceiveEndpoint(QueueConstants.CommunicationConstants.SendMailQueue, ev =>
        {
            ev.Consumer<MailConsumer>();
        });
    }));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
