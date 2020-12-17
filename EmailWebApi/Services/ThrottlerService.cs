using EmailWebApi.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Services
{
    public class ThrottlerService : IThrottlerService<Guid>
    {
        private IOptions<ThrottlerSettings> _settings;

        private static Queue<Task<Guid>> tasks;

        private static int counter;
        private static DateTime startPoint = DateTime.Now;
        private static DateTime endPoint = startPoint.AddMinutes(2);
        public ThrottlerService(IOptions<ThrottlerSettings> settings)
        {
            _settings = settings;

            tasks = new Queue<Task<Guid>>();
        }
        public async Task<Guid> Invoke(Task<Guid> func)
        {
            //var endPoint = startPoint.AddMinutes(2);
            if (counter < _settings.Value.GlobalRequestPerMinute && DateTime.Now < endPoint)
            {
                counter++;
                return await func;
            }
            else if (counter >= _settings.Value.GlobalRequestPerMinute && startPoint >= endPoint)
            {
                startPoint = endPoint;
                counter = 0;
                tasks.Enqueue(func);
                return Guid.Empty;
            }
            else
            {
                if (tasks.Count != 0)
                {
                    return await tasks.Dequeue();
                }
                tasks.Enqueue(func);
                return Guid.Empty;
            }
        }
    }
}
