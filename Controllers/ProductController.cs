using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Models;
using WebStore.Services.Implements;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using WebStore.Models.ViewModels;

namespace WebStore.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IDbRepository<Product> _dataRepository;

    // GET
    public ProductController(ApplicationDbContext db)
    {
        _db = db;
        _dataRepository = new PostgreDbRepository<Product>(_db);
    }

    public async Task<IActionResult> Index()
    {
        var list = await _dataRepository.GetJoinAllEntiesAsync();
        return View(list);
    }

    public async Task<IActionResult> AddEdit(int id)
    {
        if (id <= 0)
        {
            return NotFound();
        }
        Task<Product> productTask = _dataRepository.GetJoinEntiesByIdAsync(id);
        var categoriesTask = new PostgreDbRepository<Category>(_db).GetAllAsync();
        var applicationsTask = new PostgreDbRepository<ApplicationType>(_db).GetAllAsync();

        await Task.WhenAll(productTask, categoriesTask, applicationsTask);

        ProductVM productVm = new ProductVM()
        {
            Product = productTask.Result,
            AplicationSelectList = categoriesTask.Result.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }),
            CategorySelectList = applicationsTask.Result.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            })
        };
            
        return View(productVm);
        // string accountId = "FW25bXv";
        // string url = $"https://api.upload.io/v2/accounts/{accountId}/uploads/binary";
        //
        // var client = new HttpClient();
        // client.DefaultRequestHeaders.Add("Authorization", "Bearer public_FW25bXv5ssPk8WyPWL2aUmV1CR3N");
        //
        // var filePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\" +
        //                "GitHub.png";
        // var fileBytes = System.IO.File.ReadAllBytes(filePath);
        // var content = new ByteArrayContent(fileBytes);
        //
        // if (Path.GetExtension(filePath).ToLower() == ".png")
        // {
        //     content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
        // }
        // else if (Path.GetExtension(filePath).ToLower() == ".jpeg" || Path.GetExtension("path/to/image").ToLower() == ".jpg")
        // {
        //     content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
        // }
        //
        // var response =
        //     await client.PostAsync("https://api.upload.io/v2/accounts/FW25bXv/uploads/binary?folderPath=/uploads",
        //         content);
        // var json = await response.Content.ReadAsStringAsync();
        // JObject responseObject = JObject.Parse(json);
        // string path = (string)responseObject["filePath"]
    }

    [HttpPost]
    public IActionResult AddEdit(ProductVM productVm)
    {
        if (!ModelState.IsValid)
        {
            return View(productVm);
        }

        //  СОЗДАЁМ
        if (productVm.Product.Id == 0)
        {
            
        }
        //  РЕДАКТИРУЕМ
        else
        {
            
        }
    }

    public async Task<IActionResult> Delete()
    {
        // string accountId = "FW25bXv";
        // string url = $"https://api.upload.io/v2/accounts/{accountId}/files?filePath=/uploads/file-6rjF.txt";
        //
        // var client = new HttpClient();
        // client.DefaultRequestHeaders.Add("Authorization", "Bearer secret_FW25bXv77j4Xpq5xPL7rrNdohFtR");
        // var response =
        //     await client.DeleteAsync(url);
        // var json = await response.Content.ReadAsStringAsync();
        //

        return View();
    }
}