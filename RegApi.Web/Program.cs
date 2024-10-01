using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RegApi.Domain.Entities;
using RegApi.Repository.Context;
using RegApi.Repository.Handlers;
using RegApi.Repository.Models;
using RegApi.Repository.Models.BlobModels;
using RegApi.Repository.Utility.Registrations;
using RegApi.Services.Utility.Exceptions;
using RegApi.Services.Utility.Registrations;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var keyVaultUri = new Uri(builder.Configuration["KeyVaultUri"]!);
builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential(), new AzureKeyVaultConfigurationOptions
{
    ReloadInterval = TimeSpan.FromMinutes(5)
});

string testJson = "{\"test\": { \"test2\": \"test3\" }, \"test1\": { \"test2\": \"test3\" }}";
var test = JsonConvert.DeserializeObject<Dictionary<string, object>>(testJson);
var testNested = test["test"].ToString();


var connectionStringJson = builder.Configuration["ConnectionStringAzure"];
var connectionString = JsonConvert.DeserializeObject<Dictionary<string, object>>(connectionStringJson!);
builder.Services.AddDbContext<DatabaseContext>(opts =>
    opts.UseSqlServer($"Server={connectionString!["Server"]};" +
                      $"Initial Catalog={connectionString!["InitialCatalog"]};" +
                      $"Encrypt={connectionString!["Encrypt"]};" +
                      $"TrustServerCertificate={connectionString!["TrustServerCertificate"]};" +
                      $"Connection Timeout={connectionString!["ConnectionTimeout"]};" +
                      $"Authentication={connectionString!["Authentication"]}"));

builder.Services.AddIdentity<User, Role>(opt =>
{
    opt.Password.RequiredLength = 7;
})
.AddEntityFrameworkStores<DatabaseContext>()
.AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
    opt.TokenLifespan = TimeSpan.FromHours(2));

var jwtSettingsJson = builder.Configuration["JWTSettings"];
var jwtSettings = JsonConvert.DeserializeObject<JwtSettings>(jwtSettingsJson!);
var googleAuthenticationJson = builder.Configuration["GoogleAuthentication"];
var googleAuthentication = JsonConvert.DeserializeObject<Dictionary<string, object>>(googleAuthenticationJson!);
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    builder.Configuration.Bind("CookieSettings", options);
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.ValidIssuer.ToString(),
        ValidAudience = jwtSettings.ValidAudience.ToString(),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey.ToString()!))
    };
    builder.Configuration.Bind("JwtSettings", options);
})
.AddGoogle(options =>
{
    options.ClientId = googleAuthentication!["ClientId"].ToString()!;
    options.ClientSecret = googleAuthentication["ClientSecret"].ToString()!;
    options.CallbackPath = googleAuthentication["CallbackPath"].ToString();
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OnlyAdminUsers",
        policy => policy.RequireRole("Admin"));
});

builder.Services.AddSingleton<JwtHandler>();

var emailConfigJson = builder.Configuration["EmailConfiguration"];
var emailConfig = JsonConvert.DeserializeObject<EmailConfiguration>(emailConfigJson!);

var blobStorageJson = builder.Configuration["BlobStorage"];
var blobStorage = JsonConvert.DeserializeObject<BlobConfigurationModel>(blobStorageJson!);

builder.Services.AddSingleton(emailConfig);
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddSingleton(blobStorage);


builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddRepositoryLayerRegistration();
builder.Services.AddServiceLayerRegistration();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCustomExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
