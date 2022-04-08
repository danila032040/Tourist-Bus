using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TouristBusApp.Models;

namespace TouristBusApp.Services.Repositories
{
    public class TourRepository : IRepository<Tour>
    {
        private readonly string _fileName;

        public TourRepository(string fileName)
        {
            _fileName = fileName;
        }
        public void Create(Tour entity)
        {
            string json = File.ReadAllText(_fileName);
            var toursInFile = JsonConvert.DeserializeObject<List<Tour>>(json);

            while(toursInFile.Any(t=>t.Id == entity.Id)) entity.Id = new Random().Next();
            
            toursInFile.Add(entity);
            json = JsonConvert.SerializeObject(toursInFile);
            File.WriteAllText(_fileName, json);
        }

        public IEnumerable<Tour> Read()
        {
            string json = File.ReadAllText(_fileName);
            return JsonConvert.DeserializeObject<IEnumerable<Tour>>(json);
        }

        public void Update(Tour entity)
        {
            string json = File.ReadAllText(_fileName);
            var toursInFile = JsonConvert.DeserializeObject<List<Tour>>(json);

            int index = toursInFile.FindIndex(t => t.Id == entity.Id);

            toursInFile[index] = entity;
            
            json = JsonConvert.SerializeObject(toursInFile);
            File.WriteAllText(_fileName, json);
        }

        public void Delete(int id)
        {
            string json = File.ReadAllText(_fileName);
            var toursInFile = JsonConvert.DeserializeObject<List<Tour>>(json);

            toursInFile.RemoveAt(toursInFile.FindIndex(t => t.Id == id));
            
            json = JsonConvert.SerializeObject(toursInFile);
            File.WriteAllText(_fileName, json);
        }
    }
}