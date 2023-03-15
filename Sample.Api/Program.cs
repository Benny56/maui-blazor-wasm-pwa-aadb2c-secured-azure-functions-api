using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Sample.Api.Middleware;


var host = new HostBuilder()
    .ConfigureAppConfiguration(builder => builder.AddJsonFile("appsettings.json", optional: false))
    .ConfigureFunctionsWorkerDefaults(builder => builder.UseMiddleware<JwtValidationMiddleware>())
    .Build();

host.Run();
