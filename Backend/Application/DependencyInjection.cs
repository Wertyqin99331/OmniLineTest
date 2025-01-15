using Application.Data;
using Application.Data.Interceptors;
using Application.Services.ContactsService;
using Application.Services.CounterpartService;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddSingleton<AuditingInterceptor>();
		services.AddDbContext<AppDbContext>((sp, options) =>
		{
			options.AddInterceptors(sp.GetRequiredService<AuditingInterceptor>());
		});

		services.AddScoped<IContactService, ContactService>();
		services.AddScoped<ICounterpartService, CounterpartService>();

		return services;
	}
}