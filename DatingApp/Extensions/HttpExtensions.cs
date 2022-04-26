using System.Text.Json;
using DatingApp.Helpers;

namespace DatingApp.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, 
            int currentPage, int itemsPerPage, 
            int totalItems, int totalPages)
        {
            var PaginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            var options = new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            response.Headers.Add("Pagination-Info", JsonSerializer.Serialize(PaginationHeader, options));
            // in order to add above header in http response we need to accept policy which is
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination-Info");
        }
    }
}