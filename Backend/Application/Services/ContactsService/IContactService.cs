using Application.Services.ContactsService.Dto;
using CSharpFunctionalExtensions;
using Domain.Models;

namespace Application.Services.ContactsService;

public interface IContactService
{
	Task<List<Contact>> GetAsync(Guid? counterpartId = null);
	Task<Result<Contact, string>> CreateAsync(CreateContactBody body);
	Task<UnitResult<string>> UpdateAsync(UpdateContactBody body);
	Task<Result<Contact, string>> DeleteAsync(Guid id);
}