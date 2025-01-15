using Api.Modules.Contacts.Dto;
using Application.Services.ContactsService;
using Application.Services.ContactsService.Dto;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Api.Modules.Contacts;

public static class ContactsModule
{
	public static IEndpointRouteBuilder MapContactsModule(this IEndpointRouteBuilder app)
	{
		var group = app
			.MapGroup("/contacts");

		group.MapGet("/", GetContacts)
			.Produces<List<Contact>>()
			.Produces<string>(StatusCodes.Status400BadRequest);

		group.MapPost("/", CreateContact)
			.Produces<Contact>(StatusCodes.Status201Created)
			.Produces<string>(StatusCodes.Status400BadRequest);

		group.MapPut("/{id:guid}", UpdateContact)
			.Produces(StatusCodes.Status204NoContent)
			.Produces<string>(StatusCodes.Status400BadRequest);

		group.MapDelete("/{id:guid}", DeleteContact)
			.Produces<Contact>()
			.Produces<string>(StatusCodes.Status400BadRequest);

		return app;
	}

	private static async Task<IResult> GetContacts([FromQuery] Guid? counterpartId, [FromServices] IContactService contactService)
	{
		var contacts = await contactService.GetAsync(counterpartId);
		return Results.Ok(contacts);
	}

	private static async Task<IResult> CreateContact([FromBody] CreateContactBody body, [FromServices] IContactService contactService)
	{
		var res = await contactService.CreateAsync(body);
		return res.Match(s => Results.Created($"/contacts/{s.Id}", s), Results.BadRequest);
	}

	private static async Task<IResult> UpdateContact([FromRoute] Guid id, [FromBody] UpdateContactRequest request,
		[FromServices] IContactService contactService)
	{
		var res = await contactService.UpdateAsync(new UpdateContactBody(id, request.Email, request.FullName));
		return res.Match(Results.NoContent, Results.BadRequest);
	}

	private static async Task<IResult> DeleteContact([FromRoute] Guid id, [FromServices] IContactService contactService)
	{
		var res = await contactService.DeleteAsync(id);
		return res.Match(Results.Ok, Results.BadRequest);
	}
}