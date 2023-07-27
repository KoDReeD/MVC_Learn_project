using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Services.Implements;

public class PostgreDbRepository<T> : IDbRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    private readonly DbSet<T> _dbSet;

    public PostgreDbRepository(ApplicationDbContext db)
    {
        _db = db;
        _dbSet = _db.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        //  У конкретной таблицы вызываем метод
        return await _dbSet.ToListAsync();
    }

    public T GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public StatusCodeResult Create(T entity)
    {
        try
        {
            _dbSet.Add(entity);
            _db.SaveChanges();
            return new StatusCodeResult(200);
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"{DateTime.Now} : {dbEx.Message}");
            return new StatusCodeResult(400);
        }
    }

    public StatusCodeResult Update(T entity)
    {
        try
        {
            _dbSet.Update(entity);
            _db.SaveChanges();
            return new StatusCodeResult(200);
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"{DateTime.Now} : {dbEx.Message}");
            return new StatusCodeResult(400);
        }
    }

    public StatusCodeResult Delete(T entity)
    {
        if (entity == null)
        {
            return new StatusCodeResult(400);
        }

        try
        {
            _dbSet.Remove(entity);
            _db.SaveChanges();
            return new StatusCodeResult(200);
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"{DateTime.Now} : {dbEx.Message}");
            return new StatusCodeResult(400);
        }
    }

    public async Task<IEnumerable<T>> GetJoinAllEntiesAsync()
    {
        List<T> list;
        try
        {
            var navigationProperties = typeof(T).GetProperties()
                .Where(p => p.PropertyType.IsClass && !p.PropertyType.IsArray && p.PropertyType != typeof(string));

            foreach (var navigationProperty in navigationProperties)
            {
                await _db.Set<T>().Include(navigationProperty.Name).LoadAsync();
            }
            list = await _db.Set<T>().ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return list;
    }

    public async Task<T> GetJoinEntiesByIdAsync(int id)
    {
        T entity;
        try
        {
            var navigationProperties = typeof(T).GetProperties()
                .Where(p => p.PropertyType.IsClass && !p.PropertyType.IsArray && p.PropertyType != typeof(string));

            foreach (var navigationProperty in navigationProperties)
            {
                await _db.Set<T>().Include(navigationProperty.Name).LoadAsync();
            }

            entity = _db.Set<T>().Find(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return entity;
    }
}