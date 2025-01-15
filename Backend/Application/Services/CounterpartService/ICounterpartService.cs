using Application.Services.ContactsService.Dto;
using Application.Services.CounterpartService.Dto;
using Domain.Models;

namespace Application.Services.CounterpartService;

public interface ICounterpartService
{
	Task<List<Counterpart>> GetAsync();
	Task<Counterpart?> CreateAsync(CreateCounterpartBody body);
	Task UpdateAsync(UpdateCounterpartBody body);
	Task<Counterpart?> DeleteAsync(Guid id);
}