using Autofac;
using Autofac.Extensions.DependencyInjection;
using Backend.Data.Contexts;
using Backend.Data.Interfaces;
using Backend.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Use Autofac as the DI container
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// This is where you register Autofac modules or directly register services
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => {
    // Register your types here
    containerBuilder.RegisterType<ApplicationDbContext>()
        .AsSelf() // Register as itself
        .InstancePerLifetimeScope();
    // Register DamageRepository as an implementation of IDamageRepository
    containerBuilder.RegisterType<DamageRepository>()
        .As<IDamageRepository>() // Register as the interface
        .InstancePerLifetimeScope();
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=./Data/damages.db"));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
