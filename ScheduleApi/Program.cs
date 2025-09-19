using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application;
using ScheduleApi.ExceptionHandlers;
using ScheduleBotApi.Infrastructure.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ScheduleContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddGlobalExceptionHandlers();
builder.Services.AddAutoMapper(typeof(IApplicationAssemblyMarker).Assembly);
builder.Services.AddMediatR(typeof(IApplicationAssemblyMarker));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();