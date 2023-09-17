using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Models;
using WebStore.Services.Implements;
using System.IO;
using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WebStore.Models.ViewModels;
using WebStore.Services;

namespace WebStore.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    private readonly IDbRepository<Product> _productRepository;
    private readonly IDbRepository<Category> _categoryRepository;
    private readonly IDbRepository<ApplicationType> _appTypeRepository;

    private ProductVM _productVm { get; set; }

    private IPhotoHost _photoHost = new CloudinaryPhotoHost();

    // GET
    public ProductController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        _productRepository = new PostgreDbRepository<Product>(Helper.GetContext());
        _appTypeRepository = new PostgreDbRepository<ApplicationType>(Helper.GetContext());
        _categoryRepository = new PostgreDbRepository<Category>(Helper.GetContext());
    }

    public async Task InitProduct(int id)
    {
        var categoriesTask = _categoryRepository.GetAllAsync();
        var applicationsTask = _appTypeRepository.GetAllAsync();

        Task<Product>? productTask = id != 0 ? _productRepository.GetJoinEntiesByIdAsync(id) : null;

        if (id != 0)
        {
            await Task.WhenAll(productTask, categoriesTask, applicationsTask);
        }
        else
        {
            await Task.WhenAll(categoriesTask, applicationsTask);
        }

        Product product = productTask == null ? new Product() : productTask.Result;

        _productVm = new ProductVM()
        {
            Product = product,

            AplicationSelectList = applicationsTask.Result.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }),
            CategorySelectList = categoriesTask.Result.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            })
        };

        _productVm.OldPhotoPath = product.photoPath;
    }

    //отображение view
    public async Task<IActionResult> Index()
    {
        var list = await _productRepository.GetJoinAllEntiesAsync();
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> AddEdit(int id)
    {
        if (id < 0)
        {
            return NotFound();
        }

        await InitProduct(id);

        return View(_productVm);
    }

    [HttpPost]
    public async Task<IActionResult> AddEdit(ProductVM productVm)
    {
        ModelState.Remove("CategorySelectList");
        ModelState.Remove("AplicationSelectList");
        ModelState.Remove("Product.Category");
        ModelState.Remove("Product.Application");

        if (!ModelState.IsValid)
        {
            await InitProduct(productVm.Product.Id);
            return View(productVm);
        }
        
        string curPhoto = productVm.Product.photoPath;
        string oldPhoto = productVm.OldPhotoPath;
        
        //  Изменение фото
        if (curPhoto != oldPhoto)
        {
            //  Файл который выбрали во view
            var file = HttpContext.Request.Form.Files.FirstOrDefault();
            
            //  Удалили
            if (curPhoto == null && oldPhoto != null)
            {
                string photoPath = _photoHost.GetPhotoName(oldPhoto);
                bool deleteFlag = _photoHost.DeletePhoto(photoPath);
            }
            //  Добавили
            else if (curPhoto != null && oldPhoto == null)
            {
                var photoUrl = _photoHost.UploadPhoto(file);
                if (photoUrl == null)
                {
                    
                }
            }
            //  Изменение
            else if(curPhoto != null && oldPhoto != null)
            {
                
            }
        }

        //  СОЗДАЁМ
        if (productVm.Product.Id == 0)
        {
        }
        //  РЕДАКТИРУЕМ
        else
        {
        }

        return View("Index");
    }

    public async Task<IActionResult> Delete()
    {
        return View();
    }
}