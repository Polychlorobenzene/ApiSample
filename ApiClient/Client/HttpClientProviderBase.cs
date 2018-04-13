using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ApiClient
{
    public abstract class HttpClientProviderBase : IHttpClientProvider
    {
        private static HttpClient _staticClient;
        public HttpClient Client { get; private set; }

        public abstract string BasePath { get; }

        public HttpClientProviderBase()
        {

            Client = new HttpClient(new HttpClientHandler()
            {
                PreAuthenticate = true,
                UseDefaultCredentials = true
            })
            {
                BaseAddress = new Uri(BasePath),
                Timeout = new TimeSpan(0, 10, 0)
            };
        }

        public static HttpClient GetStaticClient(string baseAddress)
        {
            if (_staticClient == null || _staticClient.BaseAddress?.OriginalString != baseAddress)
            {
                HttpClientHandler handler = new HttpClientHandler()
                {
                    UseDefaultCredentials = true
                };
                _staticClient = new HttpClient(handler);
                _staticClient.BaseAddress = new Uri(baseAddress);
            }

            return _staticClient;
        }
    }
}
