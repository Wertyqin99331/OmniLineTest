using Domain.Models.Common;

namespace Domain.Models;

public class Contact : IAuditable
{
	public Contact(string email, string fullName, Counterpart counterpart)
	{
		this.Email = email;
		this.FullName = fullName;
	}

	private Contact()
	{
	}

	public Guid Id { get; private set; }
	public string Email { get; set; } = null!;
	public string FullName { get; set; } = null!;
	public DateTime CreatedAt { get; }
	public DateTime? ModifiedAt { get;  }


	public Counterpart Counterpart { get; set; } = null!;
}