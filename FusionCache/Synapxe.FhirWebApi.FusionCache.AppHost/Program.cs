var builder = DistributedApplication.CreateBuilder(args);
var imageRegistry = builder.Configuration["ImageRegistry"]!;

var db = builder.AddSqlServer("sqlserver")
                .WithImageRegistry(imageRegistry);
var database = db.AddDatabase("data");

var cache = builder.AddRedis("redis")
                   .WithImageRegistry(imageRegistry);

if (builder.ExecutionContext.IsRunMode)
{
    cache.WithRedisInsight(cont => cont.WithImageRegistry(imageRegistry));
}

var dacpacPath = Directory.GetFiles(
    Path.GetDirectoryName(new Projects.Synapxe_FhirWebApi_FusionCache().ProjectPath)!,
    "Ihis.FhirEngine.SqlServer.v40.dacpac",
    SearchOption.AllDirectories)[0];

var server = builder.AddProject<Projects.Synapxe_FhirWebApi_FusionCache>("server")
       .WithReference(cache)
       .WaitFor(cache)
       .WithReference(database)
       .WaitFor(database)
       .WaitForCompletion(builder.AddSqlProject("dacpac")
                                 .WithDacpac(dacpacPath)
                                 .WithReference(database))
       .WithHttpsHealthCheck("/health", 200);

//// Uncomment to enable stress testing with k6
//var k6 = builder.AddContainer("k6", "grafana/k6")
//                //.WithImageRegistry(imageRegistry)
//                .WithBindMount(Path.Combine(builder.AppHostDirectory, "k6"), "/scripts")
//                .WithArgs("run", "--insecure-skip-tls-verify", "/scripts/stress.js")
//                .WithEnvironment("AppUrl", server.GetEndpoint("https"))
//                .WaitFor(server);

await builder.Build().RunAsync();
