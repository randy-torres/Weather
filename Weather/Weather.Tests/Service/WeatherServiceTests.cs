using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Weather.Service;
using Weather.Service.DataContract;

namespace Weather.Tests.Service
{
    [TestClass]
    public class WeatherServiceTests
    {
        private WeatherService service;

        [TestInitialize]
        public void Setup()
        {
            service = new WeatherService("http://webservicex.net/globalweather.asmx", new MemoryCache("test"));
        }

        [TestMethod]
        public async Task can_get_cities_for_country()
        {
            var cities = await service.GetCitiesByCountry("Australia");

            Assert.IsNotNull(cities);
            Assert.IsTrue(cities.Any());
        }

        [TestMethod]
        public void serialisestuff()
        {
            var data = new CityCollection
            {
                Cities = new[] {new City {Country = "Australia", Name = "Sydney"}, new City {Country = "Australia", Name = "Melbourne"}  }
            };

            var serializer = new XmlSerializer(typeof (CityCollection));

            serializer.Serialize(Console.Out, data);
            Console.WriteLine();
        }
    }
}
