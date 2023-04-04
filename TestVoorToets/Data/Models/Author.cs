using System.ComponentModel.DataAnnotations;

namespace TestVoorToets.Data.Models;

public class Author
{
	// Omdat we geen Id hebben, willen wij dat EF Core Name gebruikt als Primary Key
	// Dit kan door de [Key] attribute te gebruiken
	// Als je dit niet doet, dan zal EF Core een Id property aanmaken
	// Dit betekend dat je geen dubbele namen kan hebben
	[Key, MinLength(3)]
	public string Name { get; set; } = String.Empty;
	
	public ICollection<Blog> Blogs { get; set; }
}