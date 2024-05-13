using Newtonsoft.Json;
using ReadyTechCoffee.Business.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyTechCoffee.Business
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _myClient;
        public async Task<decimal> GetTemperature()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var payload = await this._myClient.GetAsync(BusinessConstants.weatherApiUrl);
                    var json = payload.Content.ReadAsStringAsync().Result;
                    var weatherData = JsonConvert.DeserializeObject<WeatherData>(json);

                    // Access the current temperature
                    decimal currentTemperature = weatherData.Main.Temp;

                    return currentTemperature;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
