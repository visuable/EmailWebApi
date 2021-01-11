using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EmailWebApi.Controllers;
using EmailWebApi.Db.Database;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Dto;
using EmailWebApi.Db.Entities.Settings;
using EmailWebApi.Services.Classes;
using EmailWebApi.Services.Interfaces;
using EmailWebApi.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EmailWebApi.Tests.Controllers
{
    public class EmailControllerTests
    {
        public EmailControllerTests()
        {
            var services = new ServiceCollection();

            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            services.AddLogging();
            services.AddOptions();

            services.AddScoped<IStatusService, FakeStatusService>();
            services.AddScoped<IThrottlingService, FakeThrottlingService>();

            services.AddSingleton<EmailController>();

            services.Configure<SmtpSettings>(_configuration.GetSection("SmtpSettings"));
            services.Configure<ThrottlingSettings>(_configuration.GetSection("ThrottlingSettings"));

            services.AddDbContext<EmailContext>(x =>
                x.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies());
            services.AddAutoMapper(typeof(Startup));

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            services.AddHostedService<QueryExecutorService>();

            _provider = services.BuildServiceProvider();
        }

        private readonly IServiceProvider _provider;
        private readonly IConfiguration _configuration;

        [Theory]
        [ClassData(typeof(JsonRequestEmailInfoDtoGenerator))]
        public async void GetEmailState(JsonRequest<EmailInfoDto> request)
        {
            //Arrange
            var controller = _provider.GetRequiredService<EmailController>();

            //Act
            var result =
                (await controller.GetEmailState(request) as OkObjectResult)?.Value as JsonResponse<EmailStateDto>;

            //Assert
            Assert.Equal(EmailStatusDto.Sent, result?.Output.Status);
        }

        [Theory]
        [ClassData(typeof(JsonRequestEmailDtoGenerator))]
        public async void Send(JsonRequest<EmailDto> request)
        {
            //Arrange
            var controller = _provider.GetRequiredService<EmailController>();

            //Act
            var result = (await controller.Send(request) as OkObjectResult)?.Value as JsonResponse<EmailInfoDto>;

            //Assert
            Assert.Equal(Guid.Empty, result?.Output.UniversalId);
        }

        public class JsonRequestEmailDtoGenerator : IEnumerable<object[]>
        {
            private readonly object[] _data;

            public JsonRequestEmailDtoGenerator()
            {
                _data = new object[]
                {
                    new JsonRequest<EmailDto>
                    {
                        Input = new EmailDto
                        {
                            Content = new EmailContentDto
                            {
                                Address = "test@test.test",
                                Body = new EmailBodyDto
                                {
                                    Body = "test",
                                    Save = true
                                },
                                Title = "test"
                            }
                        }
                    }
                };
            }

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return _data;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class JsonRequestEmailInfoDtoGenerator : IEnumerable<object[]>
        {
            private readonly object[] _data;

            public JsonRequestEmailInfoDtoGenerator()
            {
                _data = new object[]
                {
                    new JsonRequest<EmailInfoDto>
                    {
                        Input = new EmailInfoDto
                        {
                            Date = DateTime.Now,
                            UniversalId = Guid.Empty
                        }
                    }
                };
            }

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return _data;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        [Fact]
        public async void GetApplicationState()
        {
            //Arrange
            var controller = _provider.GetRequiredService<EmailController>();

            //Act
            var result =
                (await controller.GetApplicationState() as OkObjectResult)?.Value as JsonResponse<ApplicationStateDto>;

            //Assert
            Assert.Equal(-1, result?.Output.Total);
        }
    }
}