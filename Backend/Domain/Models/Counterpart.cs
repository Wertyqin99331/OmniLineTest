using Domain.Models.Common;

namespace Domain.Models;

public class Counterpart : IAuditable
{
	public Counterpart(string name)
	{
		this.Name = name;
	}

	private Counterpart()
	{
	}

	public Guid Id { get; private set; }
	public string Name { get; set; } = null!;
	public DateTime CreatedAt { get; }
	public DateTime? ModifiedAt { get; }

	public List<Contact> Contacts { get; private set; } = [];
}