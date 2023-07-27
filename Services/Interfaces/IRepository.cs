using Microsoft.AspNetCore.Mvc;

namespace WebStore.Data;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    T GetById(int id);
    StatusCodeResult Create(T entity);
    StatusCodeResult Update(T entity);
    StatusCodeResult Delete(T entity);
}