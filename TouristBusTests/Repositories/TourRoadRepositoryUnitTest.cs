using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TouristBusApp.Models;
using TouristBusApp.Services.Repositories;
using Xunit;

namespace TouristBusTests.Repositories
{
    public class TourRoadRepositoryUnitTest
    {
        [Fact]
        public void CreateTourRoad_CorrectCreation()
        {

            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\TourRoads.json";
            string initialText = File.ReadAllText(fileName);
            try
            {
                IRepository<Road> rep = new TourRoadRepository(fileName);

                Road road = new Road
                {
                    Name = "C3",
                    DepartureTourPointId = 0,
                    ArrivalTourPointId = 1,
                    DistanceBetweenTourPoints = 100
                };
                rep.Create(road);

                string json = File.ReadAllText(fileName);
                var TourRoadsInFile = JsonConvert.DeserializeObject<List<Road>>(json);

                Assert.Contains(TourRoadsInFile, (u) => u.Equals(road));
            }
            finally
            {
                File.WriteAllText(fileName, initialText);
            }
        }

        [Fact]
        public void ReadTourRoads_CorrectReading()
        {
            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\TourRoads.json";
            IRepository<Road> rep = new TourRoadRepository(fileName);

            List<Road> TourRoadsInRep = rep.Read().ToList();
            string json = File.ReadAllText(fileName);
            var TourRoadsInFile = JsonConvert.DeserializeObject<List<Road>>(json);

            Assert.Equal(TourRoadsInFile.Count, TourRoadsInRep.Count);
            for (int i=0; i<TourRoadsInFile.Count; ++i)
            {
                Assert.Equal(TourRoadsInFile[0], TourRoadsInRep[0]);
            }
        }
        
        [Fact]
        public void UpdateTourRoad_CorrectUpdating()
        {

            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\TourRoads.json";
            string initialText = File.ReadAllText(fileName);
            try
            {
                IRepository<Road> rep = new TourRoadRepository(fileName);

                Road road = rep.Read().ElementAt(0);
                road.DistanceBetweenTourPoints = 228;
                rep.Update(road);

                string json = File.ReadAllText(fileName);
                var TourRoadsInFile = JsonConvert.DeserializeObject<List<Road>>(json);

                Assert.Contains(TourRoadsInFile, (u) => u.Equals(road));
            }
            finally
            {
                File.WriteAllText(fileName, initialText);
            }
        }
        
        [Fact]
        public void DeleteTourRoad_CorrectDelete()
        {

            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\TourRoads.json";
            string initialText = File.ReadAllText(fileName);
            
            try
            {

                IRepository<Road> rep = new TourRoadRepository(fileName);

                int idToDelete = rep.Read().FirstOrDefault((TourRoad) => TourRoad.Name == "A1").Id;
                rep.Delete(idToDelete);

                string json = File.ReadAllText(fileName);
                var TourRoadsInFile = JsonConvert.DeserializeObject<List<Road>>(json);

                Assert.DoesNotContain(TourRoadsInFile, (u) => u.Id == idToDelete);
            }
            finally
            {
                File.WriteAllText(fileName, initialText);
            }
        }
    }
}