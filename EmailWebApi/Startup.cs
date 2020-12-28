using EmailWebApi.Database;
using EmailWebApi.Objects.Settings;
using EmailWebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            services.AddScoped<IDatabaseManagerService, DatabaseManagerService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IThrottlingService, ThrottlingService>();
            services.AddScoped<IEmailTransferService, EmailTransferService>();

            services.AddLogging();
            services.AddOptions();

            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
            services.Configure<ThrottlingSettings>(Configuration.GetSection("ThrottlingSettings"));

            services.AddDbContext<EmailContext>(x =>
                x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies());

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            services.AddHostedService<QueryExecutorService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}