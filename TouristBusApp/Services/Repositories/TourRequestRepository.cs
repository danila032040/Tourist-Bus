using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TouristBusApp.Models;

namespace TouristBusApp.Services.Repositories
{
    public class TourRequestRepository : IRepository<TourRequest>
    {
        private readonly string _fileName;

        public TourRequestRepository(string fileName)
        {
            _fileName = fileName;
        }

        public void Create(TourRequest entity)
        {
            List<TourRequest> tourRequestsInFile = Read().ToList();

            while (tourRequestsInFile.Any(t => t.Id == entity.Id)) entity.Id = new Random().Next();

            tourRequestsInFile.Add(entity);
            File.WriteAllText(_fileName, JsonConvert.SerializeObject(tourRequestsInFile));
        }

        public IEnumerable<TourRequest> Read()
        {
            string json = File.ReadAllText(_fileName);
            return JsonConvert.DeserializeObject<IEnumerable<TourRequest>>(json) ?? new List<TourRequest>();
        }

        public void Update(TourRequest entity)
        {
            List<TourRequest> tourRequestsInFile = Read().ToList();

            int index = tourRequestsInFile.FindIndex(t => t.Id == entity.Id);

            tourRequestsInFile[index] = entity;

            File.WriteAllText(_fileName, JsonConvert.SerializeObject(tourRequestsInFile));
        }

        public void Delete(int id)
        {
            List<TourRequest> tourRequestsInFile = Read().ToList();

            tourRequestsInFile.RemoveAt(tourRequestsInFile.FindIndex(t => t.Id == id));

            File.WriteAllText(_fileName, JsonConvert.SerializeObject(tourRequestsInFile));
        }
    }
}