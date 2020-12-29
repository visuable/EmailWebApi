using System.Threading.Tasks;
using EmailWebApi.Entities;
using EmailWebApi.Entities.Dto;

namespace EmailWebApi.Services
{
    public interface IStatusService
    {
        Task<EmailState> GetEmailState(EmailInfo info);
        Task<ApplicationStateDto> GetApplicationState();
    }
}