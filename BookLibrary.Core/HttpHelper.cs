namespace BookLibrary.Core
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public static class HttpHelper
    {
        private static readonly HttpClient httpClient;

        static HttpHelper()
        {
            httpClient = new HttpClient();
        }

        public static async Task<T> GetApiResponse<T>(params string[] urlParts)
        {
            var response = await httpClient.GetAsync(JoinUrlParts(urlParts));

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<T>();

            return result;
        }

        private static string JoinUrlParts(params string[] urlParts)
        {
            var url = string.Concat(urlParts);

            return url;
        }
    }
}