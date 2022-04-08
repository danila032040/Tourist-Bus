using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TouristBusApp.Models;

namespace TouristBusApp.Services.Repositories
{
    public class TourPointRepository : IRepository<TourPoint>
    {
        private readonly string _fileName;

        public TourPointRepository(string fileName)
        {
            _fileName = fileName;
        }
        public void Create(TourPoint entity)
        {
            string json = File.ReadAllText(_fileName);
            var TourPointsInFile = JsonConvert.DeserializeObject<List<TourPoint>>(json);

            while(TourPointsInFile.Any(t=>t.Id == entity.Id)) entity.Id = new Random().Next();
            
            TourPointsInFile.Add(entity);
            json = JsonConvert.SerializeObject(TourPointsInFile);
            File.WriteAllText(_fileName, json);
        }

        public IEnumerable<TourPoint> Read()
        {
            string json = File.ReadAllText(_fileName);
            return JsonConvert.DeserializeObject<IEnumerable<TourPoint>>(json);
        }

        public void Update(TourPoint entity)
        {
            string json = File.ReadAllText(_fileName);
            var TourPointsInFile = JsonConvert.DeserializeObject<List<TourPoint>>(json);

            int index = TourPointsInFile.FindIndex(t => t.Id == entity.Id);

            TourPointsInFile[index] = entity;
            
            json = JsonConvert.SerializeObject(TourPointsInFile);
            File.WriteAllText(_fileName, json);
        }

        public void Delete(int id)
        {
            string json = File.ReadAllText(_fileName);
            var TourPointsInFile = JsonConvert.DeserializeObject<List<TourPoint>>(json);

            TourPointsInFile.RemoveAt(TourPointsInFile.FindIndex(t => t.Id == id));
            
            json = JsonConvert.SerializeObject(TourPointsInFile);
            File.WriteAllText(_fileName, json);
        }
    }
}