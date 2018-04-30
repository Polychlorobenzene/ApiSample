using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using WebApi;

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
        /// <summary>
        /// Gets the async Typed result from the endpoint specified in the Uri
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="requestUri"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets the async ApiResponse result from the endpoint specified in the Uri
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        protected async Task<ApiResponse> GetResultAsync(Uri requestUri)
        {
            HttpResponseMessage response;

            try
            {
                response = await _ClientProvider.Client.GetAsync(requestUri);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ApiResponse notFound = new ApiResponse();
                    notFound.StatusCode = System.Net.HttpStatusCode.NotFound;
                    notFound.Content = response.Content;
                    return notFound;
                }

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsAsync<ApiResponse>();
            }
            catch (HttpRequestException ex)
            {
                // Handle error response..
                throw new ApiException<Uri>(requestUri, ex);
            }

        }
        /// <summary>
        /// Gets the Typed result from the endpoint specified in the Uri
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="requestUri"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets the ApiResponse result from the endpoint specified in the Uri
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        protected ApiResponse GetResult(Uri requestUri)
        {
            HttpResponseMessage response;

            try
            {
                response = _ClientProvider.Client.GetAsync(requestUri).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ApiResponse notFound = new ApiResponse();
                    notFound.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return notFound;
                }

                response.EnsureSuccessStatusCode();

                return response.Content.ReadAsAsync<ApiResponse>().Result;
            }
            catch (HttpRequestException ex)
            {
                // Handle error response..
                throw new ApiException<Uri>(requestUri, ex);
            }

        }

        /// <summary>
        /// Gets the dserialized object of the TResult Type
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        protected async Task<TResult> GetWithKnownTypesAsync<TResult>(Uri requestUri)
        {
            //Not sure this does anything different than the GetAsync<TResult> method.
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

        /// <summary>
        /// Posts a value of Type TRequest and returns a response of Type TResult async
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Posts a value of Type TRequest and returns an ApiResponse async
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected async Task<ApiResponse> PostResultAsync<TRequest>(Uri requestUri, TRequest request)
        {
            try
            {
                var response = await _ClientProvider.Client.PostAsJsonAsync(requestUri, request);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ApiResponse notFound = new ApiResponse();
                    notFound.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return notFound;
                }

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsAsync<ApiResponse>();
            }
            catch (HttpRequestException ex)
            {
                // Handle error response..
                throw new ApiException<TRequest>(request, ex);
            }
        }

        /// <summary>
        /// Posts a value of Type TRequest and returns a response of Type TResult
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Posts a value of Type TRequest and returns an ApiResponse
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected ApiResponse PostResult<TRequest>(Uri requestUri, TRequest request)
        {
            try
            {
                var response = _ClientProvider.Client.PostAsJsonAsync(requestUri, request).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ApiResponse notFound = new ApiResponse();
                    notFound.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return notFound;
                }

                response.EnsureSuccessStatusCode();

                return response.Content.ReadAsAsync<ApiResponse>().Result;
            }
            catch (HttpRequestException ex)
            {
                // Handle error response..
                throw new ApiException<TRequest>(request, ex);
            }
        }

        /// <summary>
        /// Posts a value of Type TRequest and returns a deserialized TResult response async
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Builds a Uri from a string address appended to the Client BaseAddress and the Route prefix
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected Uri BuildUri(string url)
        {
            return new Uri(new Uri(_ClientProvider.Client.BaseAddress, _RoutePrefix), url);
        }

        /// <summary>
        /// Builds a Uri from the uri provided appended to the Client BaseAddress and the Route prefix
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>>
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
