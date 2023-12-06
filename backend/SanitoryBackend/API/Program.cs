using System.Reflection;
using Application.Configurations.Behaviours;
using Contracts.Interfaces;
using Infrastructure.Implements;
using MediatR;
using static Application.Features.Customers.Command.RegisterCommand;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Assembly yourHandlerAssembly = typeof(RegisterCommandHandler).Assembly;
builder.Services.AddSingleton<ISqlConnectionFactory>(x =>
    new SqlConnectionFactory("Server=localhost,1433;Database=PQ1;User Id=sa;Password=Abc@123123;TrustServerCertificate=true;Multipleactiveresultsets=true;"));
builder.Services.AddScoped(typeof(IApplicationReadDbConnection), typeof(ApplicationReadDbConnection));
builder.Services.AddScoped(typeof(IApplicationWriteDbConnection), typeof(ApplicationWriteDbConnection));
builder.Services.AddMediatR(cfg =>
    cfg
    .RegisterServicesFromAssemblies(yourHandlerAssembly));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app
              .UseCors(policy =>
                  policy
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials()
                      .WithOrigins("http://localhost:5173"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
