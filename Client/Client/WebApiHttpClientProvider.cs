using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

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

        public override string ClientKey
        {
            get
            {
                return ConfigurationManager.AppSettings["ClientKey"];
            }
        }
    }
}
