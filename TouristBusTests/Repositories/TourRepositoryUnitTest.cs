using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TouristBusApp.Models;
using TouristBusApp.Services.Repositories;
using Xunit;

namespace TouristBusTests.Repositories
{
    public class TourRepositoryUnitTest
    {
        [Fact]
        public void CreateTour_CorrectCreation()
        {
            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\Tours.json";
            string initialText = File.ReadAllText(fileName);
            try
            {
                IRepository<Tour> rep = new TourRepository(fileName);

                Tour Tour = new Tour
                {
                    Name = "TheBestTours",
                    Departure = DateTime.Now,
                    Arrival = DateTime.Today.AddDays(1),
                    TourPointIds = new List<int> {0, 1, 0}
                };
                rep.Create(Tour);

                string json = File.ReadAllText(fileName);
                var ToursInFile = JsonConvert.DeserializeObject<List<Tour>>(json);

                Assert.Contains(ToursInFile, (u) => u.Equals(Tour));
            }
            finally
            {
                File.WriteAllText(fileName, initialText);
            }
        }

        [Fact]
        public void ReadTours_CorrectReading()
        {
            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\Tours.json";
            IRepository<Tour> rep = new TourRepository(fileName);

            List<Tour> ToursInRep = rep.Read().ToList();
            string json = File.ReadAllText(fileName);
            var ToursInFile = JsonConvert.DeserializeObject<List<Tour>>(json);

            Assert.Equal(ToursInFile.Count, ToursInRep.Count);
            for (int i = 0; i < ToursInFile.Count; ++i)
            {
                Assert.Equal(ToursInFile[0], ToursInRep[0]);
            }
        }

        [Fact]
        public void UpdateTour_CorrectUpdating()
        {
            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\Tours.json";
            string initialText = File.ReadAllText(fileName);
            try
            {
                IRepository<Tour> rep = new TourRepository(fileName);

                Tour Tour = rep.Read().ElementAt(0);
                Tour.Name = "BestTourEver";
                rep.Update(Tour);

                string json = File.ReadAllText(fileName);
                var ToursInFile = JsonConvert.DeserializeObject<List<Tour>>(json);

                Assert.Contains(ToursInFile, (u) => u.Equals(Tour));
            }
            finally
            {
                File.WriteAllText(fileName, initialText);
            }
        }

        [Fact]
        public void DeleteTour_CorrectDelete()
        {
            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\Tours.json";
            string initialText = File.ReadAllText(fileName);

            try
            {
                IRepository<Tour> rep = new TourRepository(fileName);

                int idToDelete = rep.Read().FirstOrDefault((Tour) => Tour.Name == "test").Id;
                rep.Delete(idToDelete);

                string json = File.ReadAllText(fileName);
                var ToursInFile = JsonConvert.DeserializeObject<List<Tour>>(json);

                Assert.DoesNotContain(ToursInFile, (u) => u.Id == idToDelete);
            }
            finally
            {
                File.WriteAllText(fileName, initialText);
            }
        }
    }
}