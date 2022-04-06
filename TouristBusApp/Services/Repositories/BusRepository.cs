using System;
using System.Collections.Generic;
using TouristBusApp.Models;

namespace TouristBusApp.Services.Repositories
{
    public class BusRepository : IRepository<Bus>
    {
        public void Create(Bus entity)
        {
            throw new AccessViolationException("Нельзя создавать новые автобусы!");
        }

        public IEnumerable<Bus> Read()
        {
            throw new System.NotImplementedException();
        }

        public void Update(Bus entity)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}