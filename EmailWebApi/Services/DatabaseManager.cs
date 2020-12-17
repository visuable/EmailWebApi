using EmailWebApi.Database;
using EmailWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmailWebApi.Services
{
    public class DatabaseManager : IDatabaseManager
    {
        private EmailContext context;
        public DatabaseManager(EmailContext context)
        {
            this.context = context;
        }
        public Email Get(Guid key)
        {
            return context.Emails.Include(x => x.EmailStatus).Where(x => x.EmailStatus.EmailId == key).FirstOrDefault();
        }

        public int GetAllCount()
        {
            return context.Emails.ToList().Count();
        }

        public int GetCountByPredicate(Func<EmailStatus, bool> t)
        {
            return context.Emails.Include(x => x.EmailStatus).ToList().Where(x => t(x.EmailStatus)).Count();
        }

        public void Insert(Email value)
        {
            context.Emails.Add(value);
            context.SaveChanges();
        }
    }
}
