using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RegApi.Domain.Entities;
using RegApi.Repository.Context;
using RegApi.Repository.Implementations;
using RegApi.Repository.Interfaces;
using RegApi.Services.Implementations;
using RegApi.Services.Interfaces;
using RegApi.Services.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddDbContext<DatabaseContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

builder.Services.AddIdentity<User, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 7;
})
.AddEntityFrameworkStores<DatabaseContext>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
