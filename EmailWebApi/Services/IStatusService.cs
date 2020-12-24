using EmailWebApi.Objects;

namespace EmailWebApi.Services
{
    public interface IStatusService
    {
        EmailState GetEmailState(EmailInfo info);
        ApplicationState GetApplicationState();
    }
}