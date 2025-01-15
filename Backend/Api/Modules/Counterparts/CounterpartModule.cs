using Api.Modules.Counterparts.Dto;
using Application.Services.CounterpartService;
using Application.Services.CounterpartService.Dto;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Api.Modules.Counterparts;

public static class CounterpartModule
{
	public static IEndpointRouteBuilder MapCounterpartsModule(this IEndpointRouteBuilder app)
	{
		var group = app
			.MapGroup("/counterparts");

		group.MapGet("/", GetCounterparts)
			.Produces<List<Counterpart>>()
			.Produces<string>(StatusCodes.Status400BadRequest);
		
		group.MapPost("/", CreateCounterpart)
			.Produces<Counterpart>()
			.Produces<string>(StatusCodes.Status400BadRequest);
		
		group.MapPut("/{id:guid}", UpdateCounterpart)
			.Produces(StatusCodes.Status200OK)
			.Produces<string>(StatusCodes.Status400BadRequest);
			
		group.MapDelete("/{id:guid}", DeleteCounterpart)
			.Produces<Counterpart>()
			.Produces<string>(StatusCodes.Status400BadRequest);

		return app;
	}

	private static async Task<IResult> GetCounterparts([FromServices] ICounterpartService contactService)
	{
		var counterparts = await contactService.GetAsync();
		return Results.Ok(counterparts);
	}

	private static async Task<IResult> CreateCounterpart([FromBody] CreateCounterpartBody body, [FromServices] ICounterpartService contactService)
	{
		var res = await contactService.CreateAsync(body);
		return res.Match(s => Results.Created($"/counterparts/{s.Id}", s), Results.BadRequest);
	}

	private static async Task<IResult> UpdateCounterpart([FromRoute] Guid id, [FromBody] UpdateCounterpartRequest request,
		[FromServices] ICounterpartService contactService)
	{
		var res = await contactService.UpdateAsync(new UpdateCounterpartBody(id, request.Name));
		return res.Match(Results.NoContent, Results.BadRequest);
	}

	private static async Task<IResult> DeleteCounterpart([FromRoute] Guid id, [FromServices] ICounterpartService contactService)
	{
		var res = await contactService.DeleteAsync(id);
		return res.Match(Results.Ok, Results.BadRequest);
	}
}