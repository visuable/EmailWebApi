using EmailWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmailWebApi.Services
{
    public interface IDatabaseManager
    {
        void Insert(Email value);
        Email Get(Guid key);
        int GetAllCount();
        int GetCountByPredicate(Func<EmailStatus, bool> t);
    }
}
