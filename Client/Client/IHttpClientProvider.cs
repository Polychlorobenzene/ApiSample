using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ApiClient
{
    public interface IHttpClientProvider
    {
        HttpClient Client { get; }
    }
}
