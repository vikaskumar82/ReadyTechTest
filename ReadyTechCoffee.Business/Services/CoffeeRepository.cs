using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadyTechCoffee.Business.BusinessObjects;
using ReadyTechCoffee.Model;

namespace ReadyTechCoffee.Business.Services
{
    public class CoffeeRepository : ICoffeeRespository
    {
        private CoffeeContext _context;
        private IDate DateTime;
        private IWeatherService weatherService;
        public CoffeeRepository(CoffeeContext context, IDate DateTime, IWeatherService weatherService)
        {
            this._context = context;
            this.DateTime = DateTime;
            this.weatherService = weatherService;
        }

        public CoffeeItem DoMakeCoffee()
        {
            CoffeeItem coffeeItem = new CoffeeItem();

            int totalCoffeeOrder = this._context.CoffeeOrders.Count();

            //Scenario - 5th order exhausted service.
            if (totalCoffeeOrder == 4)
            {
                //coffeeItem.Message = BusinessConstants.CoffeeMachineUnavailable;
                coffeeItem.CoffeeStatus = CoffeeStatus.ServiceUnavailable;

                return coffeeItem;
            }

            //Scenario - Date check.

            if (DateTime.GetCurrentDate().Month == 4 && DateTime.GetCurrentDate().Date.Day == 1)
            {
                //coffeeItem.Message = BusinessConstants.CoffeeNotBrewing;
                coffeeItem.CoffeeStatus = CoffeeStatus.NotBrewingToday;

                return coffeeItem;
            }

            coffeeItem.Prepared = DateTime.GetCurrentDate();
            coffeeItem.CoffeeStatus = CoffeeStatus.Ready;

            var temperatureResponse = this.weatherService.GetTemperature();

            if (temperatureResponse.Result >= 30)
            {
                coffeeItem.Message = BusinessConstants.CoffeeIcedReadyMessage;
            }
            else
            {
                coffeeItem.Message = BusinessConstants.CoffeeReadyMessage;
            }

            this._context.SaveChanges();

            return coffeeItem;
        }
    }
}
