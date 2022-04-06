using System.Collections.Generic;
using TouristBusApp.Models;

namespace TouristBusApp.Services.Repositories
{
    public interface IRepository<T> where T : IBaseEntity
    {
        public void Create(T entity);
        public IEnumerable<T> Read();
        public void Update(T entity);
        public void Delete(int id);
    }
}