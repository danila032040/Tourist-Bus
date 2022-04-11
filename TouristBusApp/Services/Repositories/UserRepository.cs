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
            _fileName = fileName;
        }

        public void Create(User entity)
        {
            List<User> usersInFile = Read().ToList();

            while (usersInFile.Any(t => t.Id == entity.Id)) entity.Id = new Random().Next();

            usersInFile.Add(entity);
            File.WriteAllText(_fileName, JsonConvert.SerializeObject(usersInFile));
        }

        public IEnumerable<User> Read()
        {
            string json = File.ReadAllText(_fileName);
            return JsonConvert.DeserializeObject<IEnumerable<User>>(json) ?? new List<User>();
        }

        public void Update(User entity)
        {
            List<User> usersInFile = Read().ToList();

            int index = usersInFile.FindIndex(t => t.Id == entity.Id);

            usersInFile[index] = entity;

            File.WriteAllText(_fileName, JsonConvert.SerializeObject(usersInFile));
        }

        public void Delete(int id)
        {
            List<User> usersInFile = Read().ToList();

            usersInFile.RemoveAt(usersInFile.FindIndex(t => t.Id == id));

            File.WriteAllText(_fileName, JsonConvert.SerializeObject(usersInFile));
        }
    }
}