using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using TouristBusApp.Models;
using TouristBusApp.Services.Repositories;
using Xunit;

namespace TouristBusTests.Repositories
{
    public class BusRepositoryUnitTest
    {
        [Fact]
        public void CreateBus_ThrowsAccessViolationException()
        {
            const string resourceName = "Resources.Busses.json";
            IRepository<Bus> rep = new BusRepository(resourceName);

            Assert.Throws<AccessViolationException>(() => rep.Create(new Bus
            {
                Number = "F109",
                Capacity = 100
            }));
        }

        [Fact]
        public void ReadBusses_CorrectDataFromResource()
        {
            const string resourceName = "Resources.Busses.json";
            IRepository<Bus> rep = new BusRepository(resourceName);

            IEnumerable<Bus> bussesFromRep = rep.Read();

            var assembly = Assembly.GetExecutingAssembly();
            using Stream stream = assembly.GetManifestResourceStream("TouristBusTests." + resourceName);
            using StreamReader streamReader = new(stream);
            var bussesFromJson = JsonConvert.DeserializeObject<IEnumerable<Bus>>(streamReader.ReadToEnd());

            Assert.Equal(bussesFromJson.Count(), bussesFromRep.Count());

            for (int i = 0; i < bussesFromJson.Count(); ++i)
                Assert.Equal(bussesFromJson.ElementAt(i), bussesFromRep.ElementAt(i));
        }

        [Fact]
        public void UpdateBus_ThrowsAccessViolationException()
        {
            const string resourceName = "Resources.Busses.json";
            IRepository<Bus> rep = new BusRepository(resourceName);

            Assert.Throws<AccessViolationException>(() => rep.Update(new Bus
            {
                Id = 10,
                Capacity = 100
            }));
        }

        [Fact]
        public void DeleteBus_ThrowsAccessViolationException()
        {
            const string resourceName = "Resources.Busses.json";
            IRepository<Bus> rep = new BusRepository(resourceName);

            Assert.Throws<AccessViolationException>(() => rep.Delete(1));
        }
    }
}