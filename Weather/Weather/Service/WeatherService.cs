using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;
using Weather.Service.DataContract;

namespace Weather.Service
{
    public interface IWeatherService
    {
        Task<IEnumerable<string>> GetCitiesByCountry(string country);
    }

    public class WeatherService : IWeatherService
    {
        private readonly string serviceUrl;
        private readonly ObjectCache cache;
        private readonly string cachePrefix = typeof(WeatherService).FullName;

        public WeatherService(string serviceUrl, ObjectCache cache)
        {
            this.serviceUrl = serviceUrl;
            this.cache = cache;
        }

        public async Task<IEnumerable<string>> GetCitiesByCountry(string country)
        {
            if (string.IsNullOrEmpty(country))
            {
                throw new ArgumentNullException("country");
            }

            var cities = cache.Get(cachePrefix + "cities") as IEnumerable<City>;

            if (cities != null)
            {
                return cities.Select(x => x.Name);
            }

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(string.Format("{0}/GetCitiesByCountry?CountryName={1}", serviceUrl, country));

                var responseString = await response.Content.ReadAsStringAsync();

                var doc = XDocument.Parse(WebUtility.HtmlDecode(responseString));

                cities = (from table in doc.Descendants("Table")
                          select new City
                          {
                              Country = table.Descendants("Country").First().Value,
                              Name = table.Descendants("City").First().Value
                          });

                cache.Set(cachePrefix + "citites", cities, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromHours(3) });

                return cities.Select(x => x.Name);
            }
        }

        //did not implement get weather because the link is broken, hence cannot work out what the response format is
    }
}