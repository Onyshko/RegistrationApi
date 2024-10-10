using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using EmailSender;
using EmailSender.Models;
using EmailSender.Services;
using Newtonsoft.Json;
using RegApi.Shared.Models;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<HostOptions>(options =>
{
    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});

builder.Services.AddHostedService<Worker>();

var keyVaultUri = new Uri(builder.Configuration["KeyVaultUri"]!);
builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential(), new AzureKeyVaultConfigurationOptions
{
    ReloadInterval = TimeSpan.FromMinutes(5)
});

var emailConfigJson = builder.Configuration["EmailConfiguration"];
var emailConfig = JsonConvert.DeserializeObject<EmailConfiguration>(emailConfigJson!);

var busServiceJson = builder.Configuration["BusService"];
var busService = JsonConvert.DeserializeObject<BusServiceModel>(busServiceJson!);


builder.Services.AddSingleton(emailConfig!);
builder.Services.AddSingleton(busService!);

builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();


builder.Services.AddScoped(provider =>
{
    var busService = provider.GetRequiredService<BusServiceModel>();
    
    return new ServiceBusClient(busService.ConnectionString);
});

builder.Services.AddScoped(provider =>
{
    var busService = provider.GetRequiredService<BusServiceModel>();
    var client = new ServiceBusClient(busService.ConnectionString);

    return client.CreateReceiver(busService.EmailQueue);
});

var host = builder.Build();
host.Run();
