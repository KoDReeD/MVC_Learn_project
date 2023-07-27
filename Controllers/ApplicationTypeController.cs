using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.Models;
using WebStore.Services.Implements;

namespace WebStore.Controllers;

public class ApplicationTypeController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IRepository<ApplicationType> _dataRepository;
    
    // GET
    public ApplicationTypeController(ApplicationDbContext db)
    {
        _db = db;
        _dataRepository = new PostgreDbRepository<ApplicationType>(_db);
    }

    //  Главное представление
    public async Task<IActionResult> Index()
    {
        IEnumerable<ApplicationType> list = await _dataRepository.GetAllAsync();
        return View(list);
    }

    //  Отображение страницы AddEdit
    public IActionResult AddEdit(int id)
    {
        //  Создаём
        if (id == 0)
        {
            return View(new ApplicationType());
        }
        //  Редактируем
        else
        {
            var appType = _dataRepository.GetById(id);
            if (appType == null)
            {
                return NotFound();
            }

            return View(appType);
        }
    }

//  Сохранение объекта AddEdit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddEdit(ApplicationType appType)
    {
        if (!ModelState.IsValid)
        {
            return View(appType);
        }

        StatusCodeResult result = new BadRequestResult();

        //  Добавляем
        if (appType.Id == 0)
        {
            result = _dataRepository.Create(appType);
        }
        //  Редактируем
        else
        {
            result = _dataRepository.Update(appType);
        }

        if (result.StatusCode == 200)
        {
            return RedirectToAction("Index");
        }
        else
        {
            return View(appType);
        }
    }

    public IActionResult Delete(int id)
    {
        if (id == 0)
        {
            return NotFound();
        }

        var appType = _dataRepository.GetById(id);

        if (appType == null)
        {
            return RedirectToAction("Index");
        }
        else
        {
            return View(appType);
        }
    }

    public IActionResult DeletePost(int id)
    {
        if (id == 0)
        {
            return NotFound();
        }

        var appType = _dataRepository.GetById(id);
        if (appType == null)
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
            return View("Delete", appType);
        }
    }
}