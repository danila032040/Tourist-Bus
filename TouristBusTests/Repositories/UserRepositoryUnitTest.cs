using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TouristBusApp.Models;
using TouristBusApp.Services.Repositories;
using Xunit;

namespace TouristBusTests.Repositories
{
    public class UserRepositoryUnitTest
    {
        [Fact]
        public void CreateUser_CorrectCreation()
        {
            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\Users.json";
            string initialText = File.ReadAllText(fileName);
            try
            {
                IRepository<User> rep = new UserRepository(fileName);

                User user = new()
                {
                    Login = "admin",
                    Password = "admin"
                };
                rep.Create(user);

                string json = File.ReadAllText(fileName);
                var usersInFile = JsonConvert.DeserializeObject<List<User>>(json);

                Assert.Contains(usersInFile, u => u.Equals(user));
            }
            finally
            {
                File.WriteAllText(fileName, initialText);
            }
        }

        [Fact]
        public void ReadUsers_CorrectReading()
        {
            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\Users.json";
            IRepository<User> rep = new UserRepository(fileName);

            List<User> usersInRep = rep.Read().ToList();
            string json = File.ReadAllText(fileName);
            var usersInFile = JsonConvert.DeserializeObject<List<User>>(json);

            Assert.Equal(usersInFile.Count, usersInRep.Count);
            for (var i = 0; i < usersInFile.Count; ++i) Assert.Equal(usersInFile[0], usersInRep[0]);
        }

        [Fact]
        public void UpdateUser_CorrectUpdating()
        {
            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\Users.json";
            string initialText = File.ReadAllText(fileName);
            try
            {
                IRepository<User> rep = new UserRepository(fileName);

                User user = rep.Read().ElementAt(0);
                user.Login = "administrator";
                rep.Update(user);

                string json = File.ReadAllText(fileName);
                var usersInFile = JsonConvert.DeserializeObject<List<User>>(json);

                Assert.Contains(usersInFile, u => u.Equals(user));
            }
            finally
            {
                File.WriteAllText(fileName, initialText);
            }
        }

        [Fact]
        public void DeleteUser_CorrectDelete()
        {
            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\Users.json";
            string initialText = File.ReadAllText(fileName);

            try
            {
                IRepository<User> rep = new UserRepository(fileName);

                int idToDelete = rep.Read().FirstOrDefault(user => user.Login == "test").Id;
                rep.Delete(idToDelete);

                string json = File.ReadAllText(fileName);
                var usersInFile = JsonConvert.DeserializeObject<List<User>>(json);

                Assert.DoesNotContain(usersInFile, u => u.Id == idToDelete);
            }
            finally
            {
                File.WriteAllText(fileName, initialText);
            }
        }
    }
}