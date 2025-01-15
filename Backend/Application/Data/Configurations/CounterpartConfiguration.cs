using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Data.Configurations;

public class CounterpartConfiguration : IEntityTypeConfiguration<Counterpart>
{
	public void Configure(EntityTypeBuilder<Counterpart> builder)
	{
		builder.ToTable("Counterparts")
			.HasKey(cp => cp.Id);

		builder.HasIndex(cp => cp.Name)
			.IsUnique();

		builder.HasMany(c => c.Contacts)
			.WithOne(c => c.Counterpart);
		
		builder.Property(cp => cp.CreatedAt);
		builder.Property(cp => cp.ModifiedAt);
	}
}