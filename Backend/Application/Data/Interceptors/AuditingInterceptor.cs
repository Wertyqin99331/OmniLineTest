using Domain.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;

namespace Application.Data.Interceptors;

public class AuditingInterceptor : SaveChangesInterceptor
{
	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
		InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
		if (eventData.Context is not null)
			SetAuditProperties(eventData.Context);

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	private static void SetAuditProperties(DbContext context)
	{
		var entries = context.ChangeTracker.Entries<IAuditable>();
		var currentTime = DateTime.UtcNow;

		foreach (var entry in entries)
		{
			if (entry.State == EntityState.Added)
				SetProperty(entry.Entity, nameof(IAuditable.CreatedAt), currentTime);
			else if (entry.State == EntityState.Modified)
				SetProperty(entry.Entity, nameof(IAuditable.ModifiedAt), currentTime);
		}
	}

	private static void SetProperty(IAuditable entity, string propertyName, DateTime value)
	{
		var property = entity.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);

		if (property != null && property.CanWrite)
			property.SetValue(entity, value);
		else if (property != null && !property.CanWrite)
		{
			var backingField = entity.GetType()
				.GetField($"<{propertyName}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);

			if (backingField != null)
				backingField.SetValue(entity, value);
		}
	}
}