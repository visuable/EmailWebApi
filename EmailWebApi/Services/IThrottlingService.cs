using System.Threading.Tasks;
using EmailWebApi.Db.Entities;

namespace EmailWebApi.Services
{
    public interface IThrottlingService
    {
        Task<EmailInfo> Invoke(Email email);
    }
}