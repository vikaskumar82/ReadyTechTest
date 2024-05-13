using Microsoft.AspNetCore.Mvc;
using ReadyTechCoffee.Business.BusinessObjects;

namespace ReadyTechCoffee.Controllers
{
    [Route("api/coffee")]
    [ApiController]
    public class CoffeeController : Controller
    {
        public ICoffeeRespository CoffeeRepository { get; set; }

        public CoffeeController(ICoffeeRespository coffeeRepository)
        {
            this.CoffeeRepository = coffeeRepository;
        }


        [HttpGet("brew-coffee")]
        public IActionResult BrewCoffee()
        {
            var coffee = this.CoffeeRepository.DoMakeCoffee();
            switch(coffee.CoffeeStatus)
            {
                case CoffeeStatus.ServiceUnavailable:
                    return StatusCode(StatusCodes.Status503ServiceUnavailable);
                case CoffeeStatus.NotBrewingToday:
                    return StatusCode(StatusCodes.Status418ImATeapot);
            }

            return Ok(coffee);
        }
    }
}
