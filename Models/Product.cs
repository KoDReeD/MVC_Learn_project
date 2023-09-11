using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Models;

public class Product
{
    [Key] 
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [MaxLength(150, ErrorMessage = "The short description is too long")]
    public string ShortDescription { get; set; }
    
    [MaxLength(400, ErrorMessage = "The description is too long")]
    public string? Discription { get; set; }

    [Range(1, double.MaxValue, ErrorMessage = "Price must be greather than 0")]
    public double Price { get; set; }

    public string? photoPath { get; set; }

    [Display(Name = "Category")]
    public int CategoryId { get; set; }
    [Display(Name = "Application")]
    public int ApplicationId { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Category Category { get; set; }
    
    [ForeignKey("ApplicationId")]
    public virtual ApplicationType Application { get; set; }                                                                                                                                  
}