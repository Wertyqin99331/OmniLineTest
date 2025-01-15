using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
	: DbContext(options)
{
	public DbSet<Counterpart> Counterparts { get; set; }
	public DbSet<Contact> Contacts { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);

		optionsBuilder.UseNpgsql(configuration.GetConnectionString("Db"));
	}
}