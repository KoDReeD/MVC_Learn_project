using Microsoft.AspNetCore.Mvc;

namespace WebStore.Data;

public interface IDbRepository<T> : IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetJoinAllEntiesAsync();
    Task<T> GetJoinEntiesByIdAsync(int id);
}