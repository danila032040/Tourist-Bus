using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TouristBusApp.Models;
using TouristBusApp.Services.Repositories;
using Xunit;

namespace TouristBusTests.Repositories
{
    public class RoadRepositoryUnitTest
    {
        [Fact]
        public void CreateRoad_CorrectCreation()
        {
            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\Roads.json";
            string initialText = File.ReadAllText(fileName);
            try
            {
                IRepository<Road> rep = new RoadRepository(fileName);

                Road road = new()
                {
                    Name = "C3",
                    DepartureTourPointId = 0,
                    ArrivalTourPointId = 1,
                    DistanceBetweenTourPoints = 100
                };
                rep.Create(road);

                string json = File.ReadAllText(fileName);
                var RoadsInFile = JsonConvert.DeserializeObject<List<Road>>(json);

                Assert.Contains(RoadsInFile, u => u.Equals(road));
            }
            finally
            {
                File.WriteAllText(fileName, initialText);
            }
        }

        [Fact]
        public void ReadRoads_CorrectReading()
        {
            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\Roads.json";
            IRepository<Road> rep = new RoadRepository(fileName);

            List<Road> RoadsInRep = rep.Read().ToList();
            string json = File.ReadAllText(fileName);
            var RoadsInFile = JsonConvert.DeserializeObject<List<Road>>(json);

            Assert.Equal(RoadsInFile.Count, RoadsInRep.Count);
            for (var i = 0; i < RoadsInFile.Count; ++i) Assert.Equal(RoadsInFile[0], RoadsInRep[0]);
        }

        [Fact]
        public void UpdateRoad_CorrectUpdating()
        {
            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\Roads.json";
            string initialText = File.ReadAllText(fileName);
            try
            {
                IRepository<Road> rep = new RoadRepository(fileName);

                Road road = rep.Read().ElementAt(0);
                road.DistanceBetweenTourPoints = 228;
                rep.Update(road);

                string json = File.ReadAllText(fileName);
                var RoadsInFile = JsonConvert.DeserializeObject<List<Road>>(json);

                Assert.Contains(RoadsInFile, u => u.Equals(road));
            }
            finally
            {
                File.WriteAllText(fileName, initialText);
            }
        }

        [Fact]
        public void DeleteRoad_CorrectDelete()
        {
            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\Roads.json";
            string initialText = File.ReadAllText(fileName);

            try
            {
                IRepository<Road> rep = new RoadRepository(fileName);

                int idToDelete = rep.Read().FirstOrDefault(Road => Road.Name == "A1").Id;
                rep.Delete(idToDelete);

                string json = File.ReadAllText(fileName);
                var RoadsInFile = JsonConvert.DeserializeObject<List<Road>>(json);

                Assert.DoesNotContain(RoadsInFile, u => u.Id == idToDelete);
            }
            finally
            {
                File.WriteAllText(fileName, initialText);
            }
        }
    }
}