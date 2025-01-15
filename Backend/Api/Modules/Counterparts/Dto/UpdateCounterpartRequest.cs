using System.ComponentModel.DataAnnotations;

namespace Api.Modules.Counterparts.Dto;

public class UpdateCounterpartRequest
{
	[Required] public string Name { get; set; } = null!;
}