using Microsoft.EntityFrameworkCore;
using TestVoorToets.Data.Models;

namespace TestVoorToets.Data;

public class BlogDbContext: DbContext
{
	// We hebben een DbSet nodig voor elke model die we willen gebruiken
	// Dit zorgt ervoor dat EF Core weet dat deze model bestaat en dat deze model
	// in de database moet worden opgeslagen en uitgelezen
	public DbSet<Blog> Blogs { get; set; }
	public DbSet<Author> Authors { get; set; }
	
	public BlogDbContext(DbContextOptions<BlogDbContext> options): base(options)
	{
	}
	
	// Deze methode wordt aangeroepen wanneer de database wordt aangemaakt
	// Hier kunnen we dus onze database aanpassen en instellen hoe deze eruit moet zien
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		
		// Stel we willen dat de Content property van een Blog niet langer dan 1000 karakters mag zijn
		// Dan kunnen we dit hier instellen met de HasMaxLength methode
		modelBuilder.Entity<Blog>().Property(b => b.Content).HasMaxLength(1000);
		
		// Stel we willen dat een Author genaamd "John" altijd bestaat
		// Dan kunnen we dit hier instellen met de HasData methode
		// Eerst selecteren we de Author DbSet, dan roepen we HasData aan en geven we een nieuwe Author mee
		modelBuilder.Entity<Author>().HasData(new Author
		{
			Name = "John"
		});
		
		// We kunnen ook een lijst van objecten meegeven
		modelBuilder.Entity<Author>().HasData(new List<Author>
		{
			new Author
			{
				Name = "Jane"
			},
			new Author
			{
				Name = "Jill"
			}
		});
		
		// Ook kunnen we relaties instellen
		// Stel we willen dat een Blog altijd een Author heeft
		// Dan kunnen we dit hier instellen met de HasOne methode
		// Eerst selecteren we de Blog DbSet, dan roepen we HasOne aan en geven we een Author mee
		// Daarna roepen we WithMany aan en geven we een lijst van Blogs mee
		// Als laatste roepen we HasForeignKey aan en geven we de naam van de property mee
		// die de relatie aangeeft en hoe deze in de database moet worden opgeslagen
		modelBuilder.Entity<Blog>().HasOne(b => b.Author)
			.WithMany(a => a.Blogs)
			.HasForeignKey(b => b.AuthorName);

	}
}