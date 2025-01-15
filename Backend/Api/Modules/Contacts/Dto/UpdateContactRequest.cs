using System.ComponentModel.DataAnnotations;

namespace Api.Modules.Contacts.Dto;

public class UpdateContactRequest
{
	[Required] [EmailAddress] public string Email { get; set; } = null!;
	[Required] public string FullName { get; set; } = null!;
}