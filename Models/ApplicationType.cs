using System.ComponentModel.DataAnnotations;

namespace WebStore.Models;

public class ApplicationType
{
    [Key] 
    public int Id { get; set; }
    public string Title { get; set; }
}