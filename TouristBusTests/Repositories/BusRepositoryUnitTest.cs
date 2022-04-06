using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using TouristBusApp.Models;
using TouristBusApp.Services.Repositories;
using Xunit;
using Xunit.Sdk;

namespace TouristBustTests.Repositories
{

    public class BusRepositoryUnitTest
    {
        [Fact]
        public void ReadBusses()
        {
            const string resourceName = "TouristBusTests.Resources.Busses.json";
            IRepository<Bus> rep = new BusRepository(resourceName);
            
            IEnumerable<Bus> bussesFromRep = rep.Read();
            
            Assembly assembly = Assembly.GetExecutingAssembly();
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader streamReader = new StreamReader(stream);
            var bussesFromJson = JsonConvert.DeserializeObject<IEnumerable<Bus>>(streamReader.ReadToEnd());
            
            Assert.Equal(bussesFromJson.Count(), bussesFromRep.Count());
            
            for (int i = 0; i < bussesFromJson.Count(); ++i)
            {
                Assert.Equal(bussesFromJson.ElementAt(i), bussesFromRep.ElementAt(i));
            }
        }
    }
}