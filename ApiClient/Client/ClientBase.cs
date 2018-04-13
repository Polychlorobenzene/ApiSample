using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient
{
    public abstract class ClientBase : IDisposable
    {
        #region Private Variables
        public readonly IHttpClientProvider _ClientProvider;
        private readonly string _RoutePrefix;
        #endregion

        #region Constructors
        public ClientBase(IHttpClientProvider provider, string routePrefix)
        {
            _ClientProvider = provider;
            _RoutePrefix = routePrefix;
        }
        #endregion

        #region Functions
        protected async Task<TResult> GetAsync<TResult>(Uri requestUri)
        {
            HttpResponseMessage response;

            try
            {
                response = await _ClientProvider.Client.GetAsync(requestUri);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return default(TResult);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsAsync<TResult>();
            }
            catch (HttpRequestException ex)
            {
                // Handle error response..
                throw new ApiException<Uri>(requestUri, ex);
            }

        }

        protected TResult Get<TResult>(Uri requestUri)
        {
            HttpResponseMessage response;

            try
            {
                response = _ClientProvider.Client.GetAsync(requestUri).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return default(TResult);

                response.EnsureSuccessStatusCode();

                return response.Content.ReadAsAsync<TResult>().Result;
            }
            catch (HttpRequestException ex)
            {
                // Handle error response..
                throw new ApiException<Uri>(requestUri, ex);
            }

        }

        protected async Task<TResult> GetWithKnownTypesAsync<TResult>(Uri requestUri)
        {

            HttpResponseMessage response;

            try
            {
                response = await _ClientProvider.Client.GetAsync(requestUri);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return default(TResult);

                response.EnsureSuccessStatusCode();

                var strContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<TResult>(strContent,
                                                       new JsonSerializerSettings()
                                                       {
                                                           TypeNameHandling = TypeNameHandling.Objects,
                                                           Binder = new InheritanceSerializationBinder()
                                                       });
            }
            catch (HttpRequestException ex)
            {
                // Handle error response..
                throw new ApiException<Uri>(requestUri, ex);
            }
        }

        protected async Task<TResult> PostAsync<TRequest, TResult>(Uri requestUri, TRequest request)
        {
            try
            {
                var response = await _ClientProvider.Client.PostAsJsonAsync(requestUri, request);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return default(TResult);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsAsync<TResult>();
            }
            catch (HttpRequestException ex)
            {
                // Handle error response..
                throw new ApiException<TRequest>(request, ex);
            }
        }

        protected TResult Post<TRequest, TResult>(Uri requestUri, TRequest request)
        {
            HttpResponseMessage response;

            try
            {
                response = _ClientProvider.Client.PostAsJsonAsync(requestUri, request).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return default(TResult);

                response.EnsureSuccessStatusCode();

                return response.Content.ReadAsAsync<TResult>().Result;
            }
            catch (HttpRequestException ex)
            {
                // Handle error response..
                throw new ApiException<TRequest>(request, ex);
            }
        }

        protected async Task<TResult> PostWithKnownTypesAsync<TRequest, TResult>(Uri requestUri, TRequest request)
        {

            HttpResponseMessage response;

            try
            {
                var settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    Binder = new InheritanceSerializationBinder()
                };

                string sRequest = JsonConvert.SerializeObject(request, Formatting.None, settings);

                response = await _ClientProvider.Client.PostAsync(requestUri.AbsoluteUri, new StringContent(sRequest, UnicodeEncoding.UTF8, "application/json"));

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return default(TResult);

                response.EnsureSuccessStatusCode();

                var strContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<TResult>(strContent, settings);

            }
            catch (HttpRequestException ex)
            {
                // Handle error response..
                throw new ApiException<TRequest>(request, ex);
            }

        }

        protected Uri BuildUri(string url)
        {
            return new Uri(new Uri(_ClientProvider.Client.BaseAddress, _RoutePrefix), url);
        }

        protected Uri BuildUri(Uri url)
        {
            return new Uri(new Uri(_ClientProvider.Client.BaseAddress, _RoutePrefix), url);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _ClientProvider?.Client?.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
        #endregion
    }
}
