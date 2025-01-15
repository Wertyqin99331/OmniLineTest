namespace Application.Services.ContactsService.Dto;

public record CreateContactBody(string Email, string FullName, Guid CounterpartId);