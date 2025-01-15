using Api.Modules.Counterparts.Dto;
using Application.Services.CounterpartService;
using Application.Services.CounterpartService.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Counterparts;

public static class CounterpartModule
{
	public static IEndpointRouteBuilder MapCounterpartsModule(this IEndpointRouteBuilder app)
	{
		var group = app
			.MapGroup("/counterparts");

		group.MapGet("/", GetCounterparts);
		group.MapPost("/", CreateCounterpart);
		group.MapPut("/{id:guid}", UpdateCounterpart);
		group.MapDelete("/{id:guid}", DeleteCounterpart);

		return app;
	}

	private static async Task<IResult> GetCounterparts([FromServices] ICounterpartService contactService)
	{
		var counterparts = await contactService.GetAsync();
		return Results.Ok(counterparts);
	}

	private static async Task<IResult> CreateCounterpart([FromBody] CreateCounterpartBody body, [FromServices] ICounterpartService contactService)
	{
		var counterpart = await contactService.CreateAsync(body);
		return counterpart == null
			? Results.BadRequest("Something went wrong")
			: Results.Created($"/counterparts/{counterpart.Id}", counterpart);
	}

	private static async Task<IResult> UpdateCounterpart([FromRoute] Guid id, [FromBody] UpdateCounterpartRequest request,
		[FromServices] ICounterpartService contactService)
	{
		await contactService.UpdateAsync(new UpdateCounterpartBody(id, request.Name));
		return Results.NoContent();
	}

	private static async Task<IResult> DeleteCounterpart([FromRoute] Guid id, [FromServices] ICounterpartService contactService)
	{
		var counterpart = await contactService.DeleteAsync(id);
		return counterpart == null
			? Results.BadRequest("Something went wrong")
			: Results.Ok(counterpart);
	}
}