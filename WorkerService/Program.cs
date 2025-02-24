using AspireMessageQueuing.ServiceDefaults;
using Contracts;
using MassTransit;
using WorkerService;
using WorkerService.Consumers;

var builder = Host.CreateApplicationBuilder(args);
var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

bool withRabbitMQ = true;
bool withKafka = true;

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
            rider.AddConsumer<KafkaMessageConsumer>();

            rider.UsingKafka(
                (context, cfg) =>
                {
                    cfg.Host(builder.Configuration.GetConnectionString(Constants.KafkaConnectionName));

                    cfg.TopicEndpoint<KafkaMessage>(
                        topicName: Constants.KafkaTopicName,    // wildcards: "^topic-[0-9]*"
                        groupId: Constants.KafkaGroupId,
                        e =>
                        {
                            e.ConfigureConsumer<KafkaMessageConsumer>(context);
                        }
                    );
                }
            );
        });
    }
});

var host = builder.Build();
host.Run();
