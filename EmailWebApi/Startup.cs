using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using AutoMapper;
using EmailWebApi.Db.Database;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Settings;
using EmailWebApi.Db.Repositories;
using EmailWebApi.Services.Classes;
using EmailWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;

namespace EmailWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddLogging();
            services.AddOptions();
            services.AddSwaggerGen(options =>
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "EmailWebApi.xml");
                options.IncludeXmlComments(xmlPath, true);
            });
            services.ConfigureSwaggerGen(x => x.SwaggerDoc("emailWebApi", new OpenApiInfo
            {
                Title = "Email Web Api",
                Description = "Открытое Web Api для логгирования и отправки сообщений.",
                Version = "1b"
            }));
            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
            services.Configure<ThrottlingSettings>(Configuration.GetSection("ThrottlingSettings"));
            services.Configure<QueryExecutorSettings>(Configuration.GetSection("QueryExecutorSettings"));

            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IThrottlingService, ThrottlingService>();
            services.AddScoped<IEmailTransferService, EmailTransferService>();
            services.AddScoped<IRepository<Email>, DbContextEmailRepository>();
            services.AddScoped<IDateTimeService, SystemDateTimeService>();
            services.AddScoped<IThrottlingStateProviderService, ThrottlingStateProviderService>();
            services.AddScoped<ISmtpSenderService, SmtpSenderService>();
            services.AddScoped<ISmtpClientFactoryService, SmtpClientFactoryService>();

            services.AddDbContext<EmailContext>(x =>
                x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies());
            services.AddAutoMapper(typeof(Startup));
            services.AddHostedService<QueryExecutorService>();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/emailWebApi/swagger.json", "EmailWebApi");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSwagger();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}