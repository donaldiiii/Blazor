using BlazorMovies.Shared.DTO;
using BlazorMovies.Shared.DTOs;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public interface IAccountsRepository
    {
        Task<UserToken> Login(UserInfo userInfo);
        Task<UserToken> Register(UserInfo userInfo);
        Task<UserToken> RenewToken();
    }
}