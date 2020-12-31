using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Dto;

namespace EmailWebApi.Services
{
    public interface IStatusService
    {
        Task<EmailState> GetEmailState(EmailInfo info);
        Task<ApplicationStateDto> GetApplicationState();
    }
}