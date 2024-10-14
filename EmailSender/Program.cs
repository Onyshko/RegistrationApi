using EmailSender.Models;
using EmailSender.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RegApi.Shared.Models;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IEmailSenderService, EmailSenderService>();

        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        var emailConfigurationJson = configuration["EmailConfiguration"];
        var emailConfiguration = JsonConvert.DeserializeObject<EmailConfiguration>(emailConfigurationJson!);

        services.AddSingleton(emailConfiguration!);
    })
    .Build();

host.Run();
