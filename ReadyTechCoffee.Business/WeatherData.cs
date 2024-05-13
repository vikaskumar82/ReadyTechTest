using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyTechCoffee.Business
{
    public class WeatherData
    {
        public MainInfo Main { get; set; }
    }

    public class MainInfo
    {
        public decimal Temp { get; set; }
    }
}
