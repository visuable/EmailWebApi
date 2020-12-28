using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmailWebApi.Objects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmailWebApi.Services
{
    public class QueryExecutorService : BackgroundService
    {
        private IDatabaseManagerService _manager;
        private IEmailTransferService _email;
        public QueryExecutorService(IServiceScopeFactory scope)
        {
            using var scoped = scope.CreateScope();
            _manager = scoped.ServiceProvider.GetRequiredService<IDatabaseManagerService>();
            _email = scoped.ServiceProvider.GetRequiredService<IEmailTransferService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var queryMessages = await _manager.GetEmailsByStatus(EmailStatus.Query);
                foreach (var message in queryMessages)
                {
                    await _email.Send(message);
                    await Task.Delay(1000);
                }
            }
        }
    }
}
