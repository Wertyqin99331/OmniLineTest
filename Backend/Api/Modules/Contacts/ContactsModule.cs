using Api.Modules.Contacts.Dto;
using Application.Services.ContactsService;
using Application.Services.ContactsService.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Contacts;

public static class ContactsModule
{
	public static IEndpointRouteBuilder MapContactsModule(this IEndpointRouteBuilder app)
	{
		var group = app
			.MapGroup("/contacts");

		group.MapGet("/", GetContacts);
		group.MapPost("/", CreateContact);
		group.MapPut("/{id:guid}", UpdateContact);
		group.MapDelete("/{id:guid}", DeleteContact);

		return app;
	}

	private static async Task<IResult> GetContacts([FromQuery] Guid? counterpartId, [FromServices] IContactService contactService)
	{
		var contacts = await contactService.GetAsync(counterpartId);
		return Results.Ok(contacts);
	}

	private static async Task<IResult> CreateContact([FromBody] CreateContactBody body, [FromServices] IContactService contactService)
	{
		var contact = await contactService.CreateAsync(body);
		return contact == null
			? Results.BadRequest("Something went wrong")
			: Results.Created($"/contacts/{contact.Id}", contact);
	}

	private static async Task<IResult> UpdateContact([FromRoute] Guid id, [FromBody] UpdateContactRequest request,
		[FromServices] IContactService contactService)
	{
		await contactService.UpdateAsync(new UpdateContactBody(id, request.Email, request.FullName));
		return Results.NoContent();
	}

	private static async Task<IResult> DeleteContact([FromRoute] Guid id, [FromServices] IContactService contactService)
	{
		var contact = await contactService.DeleteAsync(id);
		return contact == null
			? Results.BadRequest("Something went wrong")
			: Results.Ok(contact);
	}
}