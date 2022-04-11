using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TouristBusApp.Models;

namespace TouristBusApp.Services.Repositories
{
    public class RoadRepository : IRepository<Road>
    {
        private readonly string _fileName;

        public RoadRepository(string fileName)
        {
            _fileName = fileName;
        }

        public void Create(Road entity)
        {
            List<Road> roadsInFile = Read().ToList();

            while (roadsInFile.Any(t => t.Id == entity.Id)) entity.Id = new Random().Next();

            roadsInFile.Add(entity);
            File.WriteAllText(_fileName, JsonConvert.SerializeObject(roadsInFile));
        }

        public IEnumerable<Road> Read()
        {
            string json = File.ReadAllText(_fileName);
            return JsonConvert.DeserializeObject<IEnumerable<Road>>(json) ?? new List<Road>();
        }

        public void Update(Road entity)
        {
            List<Road> roadsInFile = Read().ToList();

            int index = roadsInFile.FindIndex(t => t.Id == entity.Id);

            roadsInFile[index] = entity;

            File.WriteAllText(_fileName, JsonConvert.SerializeObject(roadsInFile));
        }

        public void Delete(int id)
        {
            List<Road> roadsInFile = Read().ToList();

            roadsInFile.RemoveAt(roadsInFile.FindIndex(t => t.Id == id));

            File.WriteAllText(_fileName, JsonConvert.SerializeObject(roadsInFile));
        }
    }
}