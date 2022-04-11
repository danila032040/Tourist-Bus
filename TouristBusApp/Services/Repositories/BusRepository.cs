using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using TouristBusApp.Models;

namespace TouristBusApp.Services.Repositories
{
    public class BusRepository : IRepository<Bus>
    {
        private readonly string _resourceName;

        public BusRepository(string resourceName)
        {
            _resourceName = resourceName;
        }

        public void Create(Bus entity)
        {
            throw new AccessViolationException("Нельзя создавать новые автобусы!");
        }

        public IEnumerable<Bus> Read()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            using Stream stream = assembly.GetManifestResourceStream(assembly.GetName().Name + "." + _resourceName);
            using StreamReader reader = new(stream);

            string result = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<IEnumerable<Bus>>(result) ?? new List<Bus>();
        }

        public void Update(Bus entity)
        {
            throw new AccessViolationException("Нельзя обновлять автобусы!");
        }

        public void Delete(int id)
        {
            throw new AccessViolationException("Нельзя удалять автобусы!");
        }
    }
}