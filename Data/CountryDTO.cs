using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountCountry.Data
{
    public class CountryDTO
    {
        public int Id { get; set; }
        public string countryName { get; set; }
        public string FlagName { get; set; }
        public string CountryCode { get; set; }
        public DateTime timeZone { get; set; }
        public UrlWebViewSource flag { get; set; }
    }
}
