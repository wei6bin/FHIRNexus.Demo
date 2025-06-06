using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);
var imageRegistry = builder.Configuration["ImageRegistry"]!;

var dbPasswordParam = builder.Configuration["DbPassword"] is string dbPassword ?
    builder.AddParameter("dbPassword", dbPassword, true) :
    null;
var db = builder.AddPostgres("postgres", password: dbPasswordParam, port: builder.Configuration.GetValue<int?>("DbPort"))
                .WithLifetime(dbPasswordParam is not null ? ContainerLifetime.Persistent : ContainerLifetime.Session)
                .WithImageRegistry(imageRegistry);
if (dbPasswordParam is not null)
{
    db.WithDataVolume("Synapxe_FhirWebApi_PostgresDoc_data_2025060612");
    db.WithPgAdmin(cont => cont.WithImageRegistry(imageRegistry).WithLifetime(ContainerLifetime.Persistent));
}

var database = db.AddDatabase("data");

var cache = builder.AddRedis("redis")
                   .WithImageRegistry(imageRegistry);

if (builder.Environment.EnvironmentName != "Integration" && !builder.ExecutionContext.IsPublishMode)
{
    cache.WithRedisInsight(cont => cont.WithImageRegistry(imageRegistry));
}

var server = builder.AddProject<Projects.Synapxe_FhirWebApi_PostgresDoc>("server")
       .WithReference(cache)
       .WaitFor(cache)
       .WithReference(database)
       .WaitFor(database)
       .WithHttpsHealthCheck("/health", 200);

builder.AddContainer("k6-stress-test", "grafana/k6")
       //.WithImageRegistry(imageRegistry)
       .WithBindMount(Path.Combine(builder.AppHostDirectory, "k6"), "/scripts")
       .WithArgs("run", "--insecure-skip-tls-verify", "/scripts/stress.js")
       .WithEnvironment("AppUrl", server.GetEndpoint("https"))
       .WaitFor(server)
       .WithExplicitStart();

await builder.Build().RunAsync();
