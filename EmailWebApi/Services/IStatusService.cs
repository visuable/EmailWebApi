using EmailWebApi.Objects;
using EmailWebApi.Objects.Dto;
using System.Threading.Tasks;

namespace EmailWebApi.Services
{
    public interface IStatusService
    {
        Task<EmailState> GetEmailState(EmailInfo info);
        Task<ApplicationStateDto> GetApplicationState();
    }
}