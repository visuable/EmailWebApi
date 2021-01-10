using System;
using EmailWebApi.Services.Classes;
using EmailWebApi.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EmailWebApi.Tests.Services
{
    public class SystemDateTimeServiceTests
    {
        public SystemDateTimeServiceTests()
        {
            var services = new ServiceCollection();
            services.AddScoped<IDateTimeService, SystemDateTimeService>();
            _provider = services.BuildServiceProvider();
        }

        private readonly IServiceProvider _provider;

        [Fact]
        public void Now()
        {
            //Arrange
            var service = _provider.GetRequiredService<IDateTimeService>();
            var dateTimeNow = DateTime.Now.ToShortTimeString();

            //Act
            var result = service.Now.DateTime.ToShortTimeString();

            //Assert
            Assert.Equal(dateTimeNow, result);
        }
    }
}