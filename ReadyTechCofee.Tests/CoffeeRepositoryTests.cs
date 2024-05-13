using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using ReadyTechCoffee.Business;
using ReadyTechCoffee.Business.BusinessObjects;
using ReadyTechCoffee.Business.Services;
using ReadyTechCoffee.Controllers;
using ReadyTechCoffee.Model;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace ReadyTechCofee.Tests
{

    [TestClass]
    public class CoffeeRepositoryTests
    {
        [TestMethod]
        public void TestCoffeeServiceOK()
        {
            // Arrange
            var dateMock = new Mock<IDate>();
            var weatherServiceMock = new Mock<IWeatherService>();
            dateMock.Setup(item => item.GetCurrentDate()).Returns(new DateTime(2024, 04, 10));
            var options = new DbContextOptionsBuilder<CoffeeContext>()
                       .UseInMemoryDatabase(databaseName: "CoffeeDB")
                       .Options;

            //Act
            using (var context = new CoffeeContext(options))
            {
                CoffeeRepository service = new(context, dateMock.Object, weatherServiceMock.Object);
                var result = service.DoMakeCoffee();


                //Assert
                Assert.IsTrue(result.CoffeeStatus == CoffeeStatus.Ready);
                Assert.AreEqual(result.Message, BusinessConstants.CoffeeReadyMessage);
            }
        }

        [TestMethod]
        public void TestServiceReturns418CoffeeNotBrewingToday()
        {
            // Arrange
            var dateMock = new Mock<IDate>();
            var weatherServiceMock = new Mock<IWeatherService>();
            dateMock.Setup(item => item.GetCurrentDate()).Returns(new DateTime(2024, 04, 1));
            var options = new DbContextOptionsBuilder<CoffeeContext>()
                       .UseInMemoryDatabase(databaseName: "CoffeeDB")
                       .Options;

            //Act
            using (var context = new CoffeeContext(options))
            {
                CoffeeRepository service = new(context, dateMock.Object, weatherServiceMock.Object);
                var result = service.DoMakeCoffee();


                //Assert
                Assert.IsTrue(result.CoffeeStatus == CoffeeStatus.NotBrewingToday);
            }
        }


        [TestMethod]
        public void TestServiceReturnsServiceUnavailableAfter4Hits()
        {
            // Arrange
            var dateMock = new Mock<IDate>();
            var weatherServiceMock = new Mock<IWeatherService>();
            dateMock.Setup(item => item.GetCurrentDate()).Returns(new DateTime(2024, 06, 1));
            var options = new DbContextOptionsBuilder<CoffeeContext>()
                       .UseInMemoryDatabase(databaseName: "CoffeeDB")
                       .Options;

            //Act
            using (var context = new CoffeeContext(options))
            {
                CoffeeRepository service = new(context, dateMock.Object, weatherServiceMock.Object);
                var result = service.DoMakeCoffee();
                result = service.DoMakeCoffee();
                result = service.DoMakeCoffee();
                result = service.DoMakeCoffee();
                result = service.DoMakeCoffee();

                //Assert
                Assert.IsTrue(result.CoffeeStatus == CoffeeStatus.ServiceUnavailable);
            }
        }


        [TestMethod]
        public void TestServiceDoesNotReturns418CoffeeNotBrewingTodayAfter2Hits()
        {
            // Arrange
            var dateMock = new Mock<IDate>();
            var weatherServiceMock = new Mock<IWeatherService>();
            dateMock.Setup(item => item.GetCurrentDate()).Returns(new DateTime(2024, 06, 1));
            var options = new DbContextOptionsBuilder<CoffeeContext>()
                       .UseInMemoryDatabase(databaseName: "CoffeeDB")
                       .Options;

            //Act
            using (var context = new CoffeeContext(options))
            {
                CoffeeRepository service = new(context, dateMock.Object, weatherServiceMock.Object);
                var result = service.DoMakeCoffee();
                result = service.DoMakeCoffee();


                //Assert
                Assert.IsFalse(result.CoffeeStatus == CoffeeStatus.NotBrewingToday);
                Assert.IsTrue(result.CoffeeStatus == CoffeeStatus.Ready);
            }
        }

        [TestMethod]
        public void TestCoffeeServiceTemperature()
        {
            // Arrange
            var dateMock = new Mock<IDate>();
            var weatherServiceMock = new Mock<IWeatherService>();
            weatherServiceMock.Setup(x => x.GetTemperature()).ReturnsAsync(30);
            dateMock.Setup(item => item.GetCurrentDate()).Returns(new DateTime(2024, 04, 10));
            var options = new DbContextOptionsBuilder<CoffeeContext>()
                       .UseInMemoryDatabase(databaseName: "CoffeeDB")
                       .Options;

            //Act
            using (var context = new CoffeeContext(options))
            {
                CoffeeRepository service = new(context, dateMock.Object, weatherServiceMock.Object);
                var result = service.DoMakeCoffee();


                //Assert
                Assert.IsTrue(result.CoffeeStatus == CoffeeStatus.Ready);
                Assert.AreEqual(result.Message, BusinessConstants.CoffeeIcedReadyMessage);
            }
        }
    }
}