using System.Threading.Tasks;
using EmailWebApi.Objects;

namespace EmailWebApi.Services
{
    public interface IEmailTransferService
    {
        Task Send(Email email);
    }
}