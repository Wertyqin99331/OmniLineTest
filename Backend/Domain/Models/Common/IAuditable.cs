namespace Domain.Models.Common;

public interface IAuditable
{
	DateTime CreatedAt { get; }
	DateTime? ModifiedAt { get; }
}