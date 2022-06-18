using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Mvc.Models
{
    public class WeatherData
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC * 9.0/5.0);
        public string Summary { get; set; }
    }
}