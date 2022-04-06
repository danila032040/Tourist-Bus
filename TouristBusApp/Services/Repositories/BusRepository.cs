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
        public void Create(Bus entity)
        {
            throw new AccessViolationException("Нельзя создавать новые автобусы!");
        }

        public IEnumerable<Bus> Read()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "Resources/Busses.txt";
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new StreamReader(stream);
            
            string result = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<IEnumerable<Bus>>(result);
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