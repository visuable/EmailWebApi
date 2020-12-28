using AutoMapper;
using EmailWebApi.Database;
using EmailWebApi.Objects.Settings;
using EmailWebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Text;

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
            services.AddControllers();
            services.AddLogging();
            services.AddOptions();
            services.AddSwaggerGen(options =>
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "EmailWebApi.xml");
                options.IncludeXmlComments(xmlPath, true);
            });
            services.ConfigureSwaggerGen(x => x.SwaggerDoc("emailWebApi", new OpenApiInfo()
            {
                Title = "Email Web Api",
                Description = "Открытое Web Api для логгирования и отправки сообщений.",
                Version = "1b"
            }));

            services.AddScoped<IDatabaseManagerService, DatabaseManagerService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IThrottlingService, ThrottlingService>();
            services.AddScoped<IEmailTransferService, EmailTransferService>();

            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
            services.Configure<ThrottlingSettings>(Configuration.GetSection("ThrottlingSettings"));

            services.AddDbContext<EmailContext>(x =>
                x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies());
            services.AddAutoMapper(typeof(Startup));

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            services.AddHostedService<QueryExecutorService>();
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