namespace Application.Services.ContactsService.Dto;

public record UpdateContactBody(Guid Id, string Email, string FullName);