using Application.Data;
using Application.Services.CounterpartService.Dto;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.CounterpartService;

public class CounterpartService(AppDbContext dbContext) : ICounterpartService
{
	public async Task<List<Counterpart>> GetAsync()
	{
		var query = dbContext.Counterparts.AsQueryable();
		
		return await query.ToListAsync();
	}

	public async Task<Counterpart?> CreateAsync(CreateCounterpartBody body)
	{
		var counterpart = new Counterpart(body.Name);
		dbContext.Add(counterpart);
		await dbContext.SaveChangesAsync();

		return counterpart;
	}

	public async Task UpdateAsync(UpdateCounterpartBody body)
	{
		var counterpart = await dbContext.Counterparts.FirstOrDefaultAsync(c => c.Id == body.Id);
		if (counterpart == null)
			return;

		counterpart.Name = body.Name;
		dbContext.Update(counterpart);
		await dbContext.SaveChangesAsync();
	}

	public async Task<Counterpart?> DeleteAsync(Guid id)
	{
		var counterpart = dbContext.Counterparts.FirstOrDefault(c => c.Id == id);
		if (counterpart == null)
			return null;

		dbContext.Remove(counterpart);
		await dbContext.SaveChangesAsync();

		return counterpart;
	}
}