using Application.Services.ContactsService.Dto;
using Domain.Models;

namespace Application.Services.ContactsService;

public interface IContactService
{
	Task<List<Contact>> GetAsync(Guid? counterpartId = null);
	Task<Contact?> CreateAsync(CreateContactBody body);
	Task UpdateAsync(UpdateContactBody body);
	Task<Contact?> DeleteAsync(Guid id);
}