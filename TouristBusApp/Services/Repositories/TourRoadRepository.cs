using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TouristBusApp.Models;

namespace TouristBusApp.Services.Repositories
{
    public class TourRoadRepository : IRepository<Road>
    {
        private readonly string _fileName;

        public TourRoadRepository(string fileName)
        {
            _fileName = fileName;
        }
        public void Create(Road entity)
        {
            string json = File.ReadAllText(_fileName);
            var TourRoadsInFile = JsonConvert.DeserializeObject<List<Road>>(json);

            while(TourRoadsInFile.Any(t=>t.Id == entity.Id)) entity.Id = new Random().Next();
            
            TourRoadsInFile.Add(entity);
            json = JsonConvert.SerializeObject(TourRoadsInFile);
            File.WriteAllText(_fileName, json);
        }

        public IEnumerable<Road> Read()
        {
            string json = File.ReadAllText(_fileName);
            return JsonConvert.DeserializeObject<IEnumerable<Road>>(json);
        }

        public void Update(Road entity)
        {
            string json = File.ReadAllText(_fileName);
            var TourRoadsInFile = JsonConvert.DeserializeObject<List<Road>>(json);

            int index = TourRoadsInFile.FindIndex(t => t.Id == entity.Id);

            TourRoadsInFile[index] = entity;
            
            json = JsonConvert.SerializeObject(TourRoadsInFile);
            File.WriteAllText(_fileName, json);
        }

        public void Delete(int id)
        {
            string json = File.ReadAllText(_fileName);
            var TourRoadsInFile = JsonConvert.DeserializeObject<List<Road>>(json);

            TourRoadsInFile.RemoveAt(TourRoadsInFile.FindIndex(t => t.Id == id));
            
            json = JsonConvert.SerializeObject(TourRoadsInFile);
            File.WriteAllText(_fileName, json);
        } 
    }
}