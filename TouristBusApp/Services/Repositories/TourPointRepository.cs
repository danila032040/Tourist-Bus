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
            List<TourPoint> tourPointsInFile = Read().ToList();

            while (tourPointsInFile.Any(t => t.Id == entity.Id)) entity.Id = new Random().Next();

            tourPointsInFile.Add(entity);
            File.WriteAllText(_fileName, JsonConvert.SerializeObject(tourPointsInFile));
        }

        public IEnumerable<TourPoint> Read()
        {
            string json = File.ReadAllText(_fileName);
            return JsonConvert.DeserializeObject<IEnumerable<TourPoint>>(json) ?? new List<TourPoint>();
        }

        public void Update(TourPoint entity)
        {
            List<TourPoint> TourPointsInFile = Read().ToList();

            int index = TourPointsInFile.FindIndex(t => t.Id == entity.Id);

            TourPointsInFile[index] = entity;

            File.WriteAllText(_fileName, JsonConvert.SerializeObject(TourPointsInFile));
        }

        public void Delete(int id)
        {
            List<TourPoint> tourPointsInFile = Read().ToList();

            tourPointsInFile.RemoveAt(tourPointsInFile.FindIndex(t => t.Id == id));

            File.WriteAllText(_fileName, JsonConvert.SerializeObject(tourPointsInFile));
        }
    }
}