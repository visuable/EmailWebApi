using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailWebApi.Services.Classes;
using EmailWebApi.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EmailWebApi.Tests.Services
{
    public class SystemDateTimeServiceTests
    {
        private IServiceProvider _provider;

        public SystemDateTimeServiceTests()
        {
            var services = new ServiceCollection();
            services.AddScoped<IDateTimeService, SystemDateTimeService>();
            _provider = services.BuildServiceProvider();
        }
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
