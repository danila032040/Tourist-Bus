using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TouristBusApp.Models;

namespace TouristBusApp.Services.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly string _fileName;

        public UserRepository(string fileName)
        {
            _fileName = $@"{Directory.GetCurrentDirectory()}\{fileName}";
        }
        public void Create(User entity)
        {
            string json = File.ReadAllText(_fileName);
            var usersInFile = JsonConvert.DeserializeObject<List<User>>(json);

            while(usersInFile.Any(t=>t.Id == entity.Id)) entity.Id = new Random().Next();
            
            usersInFile.Add(entity);
            json = JsonConvert.SerializeObject(usersInFile);
            File.WriteAllText(_fileName, json);
        }

        public IEnumerable<User> Read()
        {
            string json = File.ReadAllText(_fileName);
            return JsonConvert.DeserializeObject<IEnumerable<User>>(json);
        }

        public void Update(User entity)
        {
            string json = File.ReadAllText(_fileName);
            var usersInFile = JsonConvert.DeserializeObject<List<User>>(json);

            int index = usersInFile.FindIndex(t => t.Id == entity.Id);

            usersInFile[index] = entity;
            
            json = JsonConvert.SerializeObject(usersInFile);
            File.WriteAllText(_fileName, json);
        }

        public void Delete(int id)
        {
            string json = File.ReadAllText(_fileName);
            var usersInFile = JsonConvert.DeserializeObject<List<User>>(json);

            usersInFile.RemoveAt(usersInFile.FindIndex(t => t.Id == id));
            
            json = JsonConvert.SerializeObject(usersInFile);
            File.WriteAllText(_fileName, json);
        }
    }
}