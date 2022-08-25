using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.DTO;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public class AccountRepository : IAccountsRepository
    {
        private readonly IHttpService httpService;
        private readonly string baseURL = "api/accounts";

        public AccountRepository(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<UserToken> Login(UserInfo userInfo)
        {
            var response = await httpService.Post<UserInfo, UserToken>($"{baseURL}/login", userInfo);
            await response.ThrowIfNotSuccessfulResponse();
            return response.Response;
        }

        public async Task<UserToken> Register(UserInfo userInfo)
        {
            var response = await httpService.Post<UserInfo, UserToken>($"{baseURL}/create", userInfo);
            await response.ThrowIfNotSuccessfulResponse();
            return response.Response;
        }

        public async Task<UserToken> RenewToken()
        {
            var response = await httpService.Get<UserToken>($"{baseURL}/renewtoken");
            await response.ThrowIfNotSuccessfulResponse();
            return response.Response;
        }
    }
}
