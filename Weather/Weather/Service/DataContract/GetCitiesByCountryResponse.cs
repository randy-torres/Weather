using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Weather.Service.DataContract
{
    [Serializable]
    [XmlRoot(ElementName = "string", IsNullable = false, Namespace = "http://www.webserviceX.NET")]
    public class CityCollection
    {
        [XmlArray("NewDataSet")]
        [XmlArrayItem("Table", typeof(City))]
        public City[] Cities { get; set; }
    }

    [Serializable]
    public class City
    {
        [XmlElement("Country")]
        public string Country { get; set; }

        [XmlElement("City")]
        public string Name { get; set; }
    }
}