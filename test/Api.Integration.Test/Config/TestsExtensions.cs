using System.Net.Http;
using System.Net.Http.Headers;

namespace Api.Integration.Test.Config
{
    public static class TestsExtensions
    {
        public static void SetToken(this HttpClient client, string token)
        {
            client.SetJsonMediaType();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public static void SetJsonMediaType(this HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
