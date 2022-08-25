using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace BlazorMovies.Server.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertPaginationParametersInResponse<T>(this HttpContext httpcontext,
            IQueryable<T> queryable, int recordsPerPage)
        {
            if (httpcontext == null)
            {
                throw new ArgumentNullException(nameof(httpcontext));
            }
            double count = await queryable.CountAsync();
            double totalAmountOfPages = Math.Ceiling(count / recordsPerPage);
            httpcontext.Response.Headers.Add("totalAmountPages", totalAmountOfPages.ToString());
        }
    }
}
