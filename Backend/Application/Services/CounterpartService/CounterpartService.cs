using Application.Data;
using Application.Services.CounterpartService.Dto;
using CSharpFunctionalExtensions;
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

	public async Task<Result<Counterpart, string>> CreateAsync(CreateCounterpartBody body)
	{
		if (await dbContext.Counterparts.AnyAsync(c => c.Name == body.Name))
			return Result.Failure<Counterpart, string>("Контрагент с таким названием уже существует");

		var counterpart = new Counterpart(body.Name);
		dbContext.Counterparts.Add(counterpart);
		await dbContext.SaveChangesAsync();

		return counterpart;
	}

	public async Task<UnitResult<string>> UpdateAsync(UpdateCounterpartBody body)
	{
		var counterpart = await dbContext.Counterparts.FirstOrDefaultAsync(c => c.Id == body.Id);
		if (counterpart == null)
			return UnitResult.Failure("Контрагент не найден");
		
		if (await dbContext.Counterparts.AnyAsync(c => c.Name == body.Name && c.Id != body.Id))
			return UnitResult.Failure("Контрагент с таким названием уже существует");

		counterpart.Name = body.Name;
		dbContext.Counterparts.Update(counterpart);
		await dbContext.SaveChangesAsync();
		
		return UnitResult.Success<string>();
	}

	public async Task<Result<Counterpart, string>> DeleteAsync(Guid id)
	{
		var counterpart = dbContext.Counterparts.FirstOrDefault(c => c.Id == id);
		if (counterpart == null)
			return Result.Failure<Counterpart, string>("Контрагент не найден");

		dbContext.Counterparts.Remove(counterpart);
		await dbContext.SaveChangesAsync();

		return counterpart;
	}
}