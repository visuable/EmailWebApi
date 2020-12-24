using EmailWebApi.Objects;

namespace EmailWebApi.Services
{
    public interface IEmailTransferService
    {
        Email Send(Email email);
    }
}