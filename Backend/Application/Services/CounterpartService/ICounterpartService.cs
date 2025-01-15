using Application.Services.ContactsService.Dto;
using Application.Services.CounterpartService.Dto;
using CSharpFunctionalExtensions;
using Domain.Models;

namespace Application.Services.CounterpartService;

public interface ICounterpartService
{
	Task<List<Counterpart>> GetAsync();
	Task<Result<Counterpart, string>> CreateAsync(CreateCounterpartBody body);
	Task<UnitResult<string>> UpdateAsync(UpdateCounterpartBody body);
	Task<Result<Counterpart, string>> DeleteAsync(Guid id);
}