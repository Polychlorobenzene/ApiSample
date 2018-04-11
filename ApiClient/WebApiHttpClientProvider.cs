using System;
using System.Collections.Generic;
using System.Text;

namespace ApiClient
{
    public sealed class WebApiHttpClientProvider : HttpClientProviderBase, IHttpClientProvider
    {
        public override string BasePath
        {
            get
            {
                return @"http://localhost:57499/";
            }
        }
    }
}
