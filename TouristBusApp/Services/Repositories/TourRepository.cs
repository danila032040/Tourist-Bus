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
            List<Tour> toursInFile = Read().ToList();

            while (toursInFile.Any(t => t.Id == entity.Id)) entity.Id = new Random().Next();

            toursInFile.Add(entity);
            File.WriteAllText(_fileName, JsonConvert.SerializeObject(toursInFile));
        }

        public IEnumerable<Tour> Read()
        {
            string json = File.ReadAllText(_fileName);
            return JsonConvert.DeserializeObject<IEnumerable<Tour>>(json) ?? new List<Tour>();
        }

        public void Update(Tour entity)
        {
            List<Tour> toursInFile = Read().ToList();

            int index = toursInFile.FindIndex(t => t.Id == entity.Id);

            toursInFile[index] = entity;

            File.WriteAllText(_fileName, JsonConvert.SerializeObject(toursInFile));
        }

        public void Delete(int id)
        {
            List<Tour> toursInFile = Read().ToList();

            toursInFile.RemoveAt(toursInFile.FindIndex(t => t.Id == id));

            File.WriteAllText(_fileName, JsonConvert.SerializeObject(toursInFile));
        }
    }
}