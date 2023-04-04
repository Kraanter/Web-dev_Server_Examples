using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestVoorToets.Data.Models;

public class Blog
{
	public int Id { get; set; }
    
	[Required, MaxLength(100)]
	public string Title { get; set; }
    
	public string Content { get; set; }
    
	public string AuthorName { get; set; }
    
	[ForeignKey("AuthorName")]
	public Author Author { get; set; }
}