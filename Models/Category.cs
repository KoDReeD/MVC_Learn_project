using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [DisplayName("Display Order")]
    [Required]
    [RegularExpression(@"^\d+$", ErrorMessage = "Display Order must be a number")]
    [Range(1, Int32.MaxValue, ErrorMessage = "Display Order must be greater than 0")]
    public string DisplayOrder { get; set; }
}