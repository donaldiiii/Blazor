using BlazorMovies.Shared.DTO;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Helpers
{
    public static class IHttpServiceExtensionMethod
    {
        public static async Task<T> GetHelper<T>(this IHttpService httpService, string url)
        {
            var response = await httpService.Get<T>(url);
            await response.ThrowIfNotSuccessfulResponse();
            return response.Response;
        }

        public static async Task<PaginatedResponse<T>> GetHelper<T>(this IHttpService httpService, string url, PaginationDTO paginationDTO)
        {
            string newURL = "";
            if (url.Contains("?"))
            {
                newURL = $"{url}&page={paginationDTO.Page}&RecordsPerPage={paginationDTO.RecordsPerPage}";
            }
            else
            {
                newURL = $"{url}?page={paginationDTO.Page}&RecordsPerPage={paginationDTO.RecordsPerPage}";

            }
            var httpResponse = await httpService.Get<T>(newURL);
            var totalAmountPages = int.Parse(httpResponse.HttpResponseMessage.Headers.GetValues("totalAmountOfPages").FirstOrDefault());
            var paginatedResponse = new PaginatedResponse<T>
            {
                Response = httpResponse.Response,
                TotalAmountPages = totalAmountPages
            };
            return paginatedResponse;
        }
    }
}
