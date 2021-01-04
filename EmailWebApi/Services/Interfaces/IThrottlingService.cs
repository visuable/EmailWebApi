using System.Threading.Tasks;
using EmailWebApi.Db.Entities;

namespace EmailWebApi.Services.Interfaces
{
    public interface IThrottlingService
    {
        Task<EmailInfo> Invoke(Email email);
    }
}