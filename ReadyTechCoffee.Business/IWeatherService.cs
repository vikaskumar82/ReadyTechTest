using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyTechCoffee.Business
{
    public interface IWeatherService
    {
        public Task<decimal> GetTemperature();
    }
}
