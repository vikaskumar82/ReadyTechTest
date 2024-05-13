using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using ReadyTechCoffee.Business.BusinessObjects;
using ReadyTechCoffee.Controllers;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace ReadyTechCofee.Tests
{
    [TestClass]
    public class CoffeeControllerTests
    {
        [TestMethod]
        public void TestControllerReturnsOKStatusCodeAndCoffeeReady()
        {
            // Arrange
            var returnMessage = new CoffeeItem() { CoffeeStatus = CoffeeStatus.Ready, Message = "Your piping hot coffee is ready" };
            var moqCoffeeService = new Mock<ICoffeeRespository>();
            moqCoffeeService.Setup(item => item.DoMakeCoffee()).Returns(returnMessage);
            var controller = new CoffeeController(moqCoffeeService.Object);

            //Act
            var result = controller.BrewCoffee();
            var okResult = result as OkObjectResult;

            //Assert
            Assert.IsInstanceOfType<OkObjectResult>(result);
            Assert.IsNotNull(okResult);
            Assert.IsTrue(okResult.StatusCode == 200);

            Assert.AreEqual((okResult.Value as CoffeeItem).Message, returnMessage.Message);
        }

        [TestMethod]
        public void TestControllerReturns503CoffeeMachineOut()
        {
            // Arrange
            var returnMessage = new CoffeeItem() { CoffeeStatus = CoffeeStatus.ServiceUnavailable };
            var moqCoffeeService = new Mock<ICoffeeRespository>();
            moqCoffeeService.Setup(item => item.DoMakeCoffee()).Returns(returnMessage);
            var controller = new CoffeeController(moqCoffeeService.Object);

            //Act
            var result = controller.BrewCoffee();
            var statusCodeResult = result as IStatusCodeActionResult;
            
            //Assert
            Assert.IsInstanceOfType<IStatusCodeActionResult>(result);
            Assert.IsNotNull(statusCodeResult);
            Assert.IsTrue(statusCodeResult.StatusCode == StatusCodes.Status503ServiceUnavailable);
        }

        [TestMethod]
        public void TestControllerReturns418CoffeeNotBrewingToday()
        {
            // Arrange
            var returnMessage = new CoffeeItem() { CoffeeStatus = CoffeeStatus.NotBrewingToday };
            var moqCoffeeService = new Mock<ICoffeeRespository>();
            moqCoffeeService.Setup(item => item.DoMakeCoffee()).Returns(returnMessage);
            var controller = new CoffeeController(moqCoffeeService.Object);

            //Act
            var result = controller.BrewCoffee();
            var statusCodeResult = result as IStatusCodeActionResult;

            //Assert
            Assert.IsInstanceOfType<IStatusCodeActionResult>(result);
            Assert.IsNotNull(statusCodeResult);
            Assert.IsTrue(statusCodeResult.StatusCode == StatusCodes.Status418ImATeapot);
        }
    }
}