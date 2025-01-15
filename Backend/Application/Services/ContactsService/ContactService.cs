using Application.Data;
using Application.Services.ContactsService.Dto;
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

	public async Task<Contact?> CreateAsync(CreateContactBody body)
	{
		var counterpart = await dbContext.Counterparts.FirstOrDefaultAsync(c => c.Id == body.CounterpartId);
		if (counterpart == null)
			return null;

		var contact = new Contact(body.Email, body.FullName, counterpart);
		dbContext.Add(counterpart);
		await dbContext.SaveChangesAsync();

		return contact;
	}

	public async Task UpdateAsync(UpdateContactBody body)
	{
		var contact = await dbContext.Contacts.FirstOrDefaultAsync(c => c.Id == body.Id);
		if (contact == null)
			return;

		contact.Email = body.Email;
		contact.FullName = body.FullName;
		await dbContext.SaveChangesAsync();
	}

	public async Task<Contact?> DeleteAsync(Guid id)
	{
		var contact = dbContext.Contacts.FirstOrDefault(c => c.Id == id);
		if (contact == null)
			return null;

		dbContext.Remove(contact);
		await dbContext.SaveChangesAsync();

		return contact;
	}
}