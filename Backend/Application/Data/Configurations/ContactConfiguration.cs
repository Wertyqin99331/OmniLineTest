using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Data.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
	public void Configure(EntityTypeBuilder<Contact> builder)
	{
		builder.ToTable("Contacts", t =>
			{
				t.HasCheckConstraint("CK_Contacts_Email_NotEmpty", "\"Email\" <> ''");

				t.HasCheckConstraint("CK_Contacts_FullName_NotEmpty", "\"FullName\" <> ''");
			})
			.HasKey(c => c.Id);

		builder.HasIndex(c => c.Email)
			.IsUnique();

		builder.Property(c => c.CreatedAt);
		builder.Property(c => c.ModifiedAt);
	}
}