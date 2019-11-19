using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ContactsList.Helpers
{
    public static class ApiHelper
    {
        public static HttpClient ApiClient { get; set; }

        public static void InitializeClient()
        {
            ApiClient = new HttpClient(
                new HttpClientHandler()
                {
                    AutomaticDecompression = System.Net.DecompressionMethods.GZip
                });
            ApiClient.BaseAddress = new Uri("http://localhost:5000/api/contacts");
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.Timeout = new TimeSpan(0, 0, 30);
        }
    }
}
