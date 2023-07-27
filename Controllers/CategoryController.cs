using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.Models;
using WebStore.Services.Implements;

namespace WebStore.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IRepository<Category> _dataRepository;

    public CategoryController(ApplicationDbContext db)
    {
        _db = db;
        _dataRepository = new PostgreDbRepository<Category>(_db);
    }

    //  Главное представление
    public async Task<IActionResult> Index()
    {
        IEnumerable<Category> list = await _dataRepository.GetAllAsync();
        return View(list);
    }

    //  Отображение страницы AddEdit
    public IActionResult AddEdit(int id)
    {
        //  Создаём
        if (id == 0)
        {
            return View(new Category());
        }
        //  Редактируем
        else
        {
            var category = _dataRepository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
    }

//  Сохранение объекта AddEdit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddEdit(Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        StatusCodeResult result = new BadRequestResult();
        //  Добавляем
        if (category.Id == 0)
        {
            result = _dataRepository.Create(category);
        }
        //  Редактируем
        else
        {
            result = _dataRepository.Update(category);
        }

        if (result.StatusCode == 200)
        {
            return RedirectToAction("Index");
        }
        else
        {
            return View(category);
        }
    }

    public IActionResult Delete(int id)
    {
        if (id == 0)
        {
            return NotFound();
        }

        var category = _dataRepository.GetById(id);

        if (category == null)
        {
            return RedirectToAction("Index");
        }
        else
        {
            return View(category);
        }
    }

    public IActionResult DeletePost(int id)
    {
        if (id == 0)
        {
            return NotFound();
        }

        var category = _dataRepository.GetById(id);
        if (category == null)
        {
            return NotFound();
        }

        StatusCodeResult result = new BadRequestResult();
        if (result.StatusCode == 200)
        {
            return RedirectToAction("Index");
        }
        else
        {
            return View("Delete", category);
        }
    }
}