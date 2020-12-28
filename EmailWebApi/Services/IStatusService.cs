using EmailWebApi.Objects;
using System.Threading.Tasks;

namespace EmailWebApi.Services
{
    public interface IStatusService
    {
        Task<EmailState> GetEmailState(EmailInfo info);
        Task<ApplicationState> GetApplicationState();
    }
}