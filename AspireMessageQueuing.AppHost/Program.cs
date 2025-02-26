var builder = DistributedApplication.CreateBuilder(args);

var rabbitmq = builder.AddRabbitMQ(name: "rabbit-mq")
    .WithManagementPlugin()
    .WithLifetime(ContainerLifetime.Persistent);

var kafka = builder.AddKafka(name: "kafka")
    .WithKafkaUI()
    .WithLifetime(ContainerLifetime.Persistent);

var workerService = builder.AddProject<Projects.WorkerService>(name: "worker-service")
    .WithReference(rabbitmq)
    .WithReference(kafka)
    .WaitFor(rabbitmq)
    .WaitFor(kafka)
    .WithReplicas(replicas: 1);

var webApi = builder.AddProject<Projects.WebApiMessageQueues>(name: "web-api")
    .WithReference(rabbitmq)
    .WithReference(kafka)
    .WaitFor(workerService);

builder.Build().Run();
