var builder = DistributedApplication.CreateBuilder(args);

// kafka:
// dotnet add package Aspire.Hosting.Kafka
// https://learn.microsoft.com/en-us/dotnet/aspire/messaging/kafka-integration?tabs=dotnet-cli

// postgres:
// dotnet add package Aspire.Hosting.PostgreSQL
// https://learn.microsoft.com/en-us/dotnet/aspire/database/postgresql-entity-framework-integration?tabs=dotnet-cli

var rabbitmq = builder.AddRabbitMQ("RabbitMQ")
    .WithManagementPlugin()
    .WithLifetime(ContainerLifetime.Persistent);

//var kafka = builder.AddKafka("Kafka")
//    .WithLifetime(ContainerLifetime.Persistent);


var workerService = builder.AddProject<Projects.RabbitMQWorkerService>("RabbitMQWorkerService")
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReplicas(2);

var webApi = builder.AddProject<Projects.WebApiMessageQueues>("WebApi")
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WaitFor(workerService);

builder.Build().Run();
