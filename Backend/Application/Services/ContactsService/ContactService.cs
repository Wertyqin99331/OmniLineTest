using Application.Data;
using Application.Services.ContactsService.Dto;
using CSharpFunctionalExtensions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.ContactsService;

public class ContactService(AppDbContext dbContext) : IContactService
{
	public async Task<List<Contact>> GetAsync(Guid? counterpartId = null)
	{
		var query = dbContext.Contacts
			.Include(c => c.Counterpart)
			.AsQueryable();

		if (counterpartId.HasValue)
			query = query
				.Where(c => c.Counterpart.Id == counterpartId);

		return await query.ToListAsync();
	}

	public async Task<Result<Contact, string>> CreateAsync(CreateContactBody body)
	{
		var counterpart = await dbContext.Counterparts.FirstOrDefaultAsync(c => c.Id == body.CounterpartId);
		if (counterpart == null)
			return Result.Failure<Contact, string>("Контрагент не найден");

		if (await dbContext.Contacts.AnyAsync(c => c.Email == body.Email))
			return Result.Failure<Contact, string>("Контакт с таким email уже существует");

		var contact = new Contact(body.Email, body.FullName, counterpart);
		dbContext.Contacts.Add(contact);
		await dbContext.SaveChangesAsync();

		return contact;
	}

	public async Task<UnitResult<string>> UpdateAsync(UpdateContactBody body)
	{
		var contact = await dbContext.Contacts.FirstOrDefaultAsync(c => c.Id == body.Id);
		if (contact == null)
			return UnitResult.Failure("Контакт не найден");

		if (await dbContext.Contacts.AnyAsync(c => c.Email == body.Email && c.Id != body.Id))
			return UnitResult.Failure("Контакт с таким email уже существует");

		contact.Email = body.Email;
		contact.FullName = body.FullName;
		await dbContext.SaveChangesAsync();

		return UnitResult.Success<string>();
	}

	public async Task<Result<Contact, string>> DeleteAsync(Guid id)
	{
		var contact = dbContext.Contacts.FirstOrDefault(c => c.Id == id);
		if (contact == null)
			return Result.Failure<Contact, string>("Контакт не найден");

		dbContext.Contacts.Remove(contact);
		await dbContext.SaveChangesAsync();

		return contact;
	}
}