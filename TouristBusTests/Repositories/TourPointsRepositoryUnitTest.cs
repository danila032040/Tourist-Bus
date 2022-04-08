using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TouristBusApp.Models;
using TouristBusApp.Services.Repositories;
using Xunit;

namespace TouristBusTests.Repositories
{
    public class TourPointsRepositoryUnitTest
    {
        public class TourPointRepositoryUnitTest
    {
        [Fact]
        public void CreateTourPoint_CorrectCreation()
        {

            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\TourPoints.json";
            string initialText = File.ReadAllText(fileName);
            try
            {


                IRepository<TourPoint> rep = new TourPointRepository(fileName);

                TourPoint TourPoint = new TourPoint
                {
                    Name = "Россия"
                };
                rep.Create(TourPoint);

                string json = File.ReadAllText(fileName);
                var TourPointsInFile = JsonConvert.DeserializeObject<List<TourPoint>>(json);

                Assert.Contains(TourPointsInFile, (u) => u.Equals(TourPoint));
            }
            finally
            {
                File.WriteAllText(fileName, initialText);
            }
        }

        [Fact]
        public void ReadTourPoints_CorrectReading()
        {
            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\TourPoints.json";
            IRepository<TourPoint> rep = new TourPointRepository(fileName);

            List<TourPoint> TourPointsInRep = rep.Read().ToList();
            string json = File.ReadAllText(fileName);
            var TourPointsInFile = JsonConvert.DeserializeObject<List<TourPoint>>(json);

            Assert.Equal(TourPointsInFile.Count, TourPointsInRep.Count);
            for (int i=0; i<TourPointsInFile.Count; ++i)
            {
                Assert.Equal(TourPointsInFile[0], TourPointsInRep[0]);
            }
        }
        
        [Fact]
        public void UpdateTourPoint_CorrectUpdating()
        {

            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\TourPoints.json";
            string initialText = File.ReadAllText(fileName);
            try
            {
                IRepository<TourPoint> rep = new TourPointRepository(fileName);

                TourPoint TourPoint = rep.Read().ElementAt(0);
                TourPoint.Name = "Америка";
                rep.Update(TourPoint);

                string json = File.ReadAllText(fileName);
                var TourPointsInFile = JsonConvert.DeserializeObject<List<TourPoint>>(json);

                Assert.Contains(TourPointsInFile, (u) => u.Equals(TourPoint));
            }
            finally
            {
                File.WriteAllText(fileName, initialText);
            }
        }
        
        [Fact]
        public void DeleteTourPoint_CorrectDelete()
        {

            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                              @"\Data\TourPoints.json";
            string initialText = File.ReadAllText(fileName);
            
            try
            {

                IRepository<TourPoint> rep = new TourPointRepository(fileName);

                int idToDelete = rep.Read().FirstOrDefault((TourPoint) => TourPoint.Name == "Беларусь").Id;
                rep.Delete(idToDelete);

                string json = File.ReadAllText(fileName);
                var TourPointsInFile = JsonConvert.DeserializeObject<List<TourPoint>>(json);

                Assert.DoesNotContain(TourPointsInFile, (u) => u.Id == idToDelete);
            }
            finally
            {
                File.WriteAllText(fileName, initialText);
            }
        }
    }
    }
}