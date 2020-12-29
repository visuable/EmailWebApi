using System.Threading.Tasks;
using EmailWebApi.Entities;

namespace EmailWebApi.Services
{
    public interface IEmailTransferService
    {
        Task<EmailInfo> Send(Email email);
    }
}