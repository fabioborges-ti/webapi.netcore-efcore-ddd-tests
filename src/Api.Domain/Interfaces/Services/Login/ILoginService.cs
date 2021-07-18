using Api.Domain.Dtos.Login;
using System.Threading.Tasks;

namespace Api.Domain.Interfaces.Services.Login
{
    public interface ILoginService
    {
        Task<object> FindByLogin(LoginDto user);
    }
}
