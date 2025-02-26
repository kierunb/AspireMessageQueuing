using AspireMessageQueuing.ServiceDefaults;
using Contracts;
using MassTransit;
using Scalar.AspNetCore;
using WebApiMessageQueues.Extensions;

var builder = WebApplication.CreateBuilder(args);
var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();

bool withRabbitMQ = Constants.WithRabbitMQ;
bool withKafka = Constants.WithKafka;

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddOpenApi();
builder.Services.AddEndpoints(typeof(Program).Assembly);

// MassTransit
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    // By default, sagas are in-memory, but should be changed to a durable
    // saga repository.
    x.SetInMemorySagaRepositoryProvider();
    x.AddConsumers(entryAssembly);
    x.AddSagaStateMachines(entryAssembly);
    x.AddSagas(entryAssembly);
    x.AddActivities(entryAssembly);

    if (withRabbitMQ)
    {
        // RabbitMQ
        x.UsingRabbitMq(
            (context, cfg) =>
            {
                cfg.Host(builder.Configuration.GetConnectionString(Constants.RabbitMQConnectionName));
                cfg.ConfigureEndpoints(context);
            }
        );
    }

    if (withKafka)
    {
        // Kafka
        x.AddRider(rider =>
        {
            rider.AddProducer<KafkaMessage>(topicName: Constants.KafkaTopicName);

            rider.UsingKafka(
                (context, k) =>
                {
                    k.Host(builder.Configuration.GetConnectionString(Constants.KafkaConnectionName));
                }
            );
        });
    } 
});


var app = builder.Build();

app.MapDefaultEndpoints();
builder.Services.AddEndpoints(typeof(Program).Assembly);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(_ => _.Servers = []);
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();
