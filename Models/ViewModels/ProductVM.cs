using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebStore.Models.ViewModels;

public class ProductVM
{
    public Product Product { get; set; }
    public string? OldPhotoPath { get; set; }
    public IEnumerable<SelectListItem> CategorySelectList { get; set; }
    public IEnumerable<SelectListItem> AplicationSelectList { get; set; }
}