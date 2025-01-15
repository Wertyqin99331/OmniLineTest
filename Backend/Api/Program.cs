using Api.Middleware;
using Api.Modules.Contacts;
using Api.Modules.Counterparts;
using Application;
using Application.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddApplicationServices();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapContactsModule();

app.MapCounterpartsModule();

await using (var scope = app.Services.CreateAsyncScope())
{
	var services = scope.ServiceProvider;
	var dbContext = services.GetRequiredService<AppDbContext>();

	await dbContext.Database.MigrateAsync();
}

app.Run();