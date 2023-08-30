using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountCountry.Data
{
    internal class CountryEntity
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public DateTime StarDateCount { get; set; }
        public DateTime EndDateCount { get; set; }
        public DateTime InsertDate { get; set; }
        public string BucktList { get; set; }
        public string TotalDaysStay { get; set; }
        public string SharedStayCount { get; set; }
        public UrlWebViewSource flag { get; set; }
    }
}
