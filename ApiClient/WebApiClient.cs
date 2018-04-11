using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using WebApi;

namespace ApiClient
{
    public class WebApiClient : ClientBase
    {
        public WebApiClient() : base(new WebApiHttpClientProvider(), "api/Values") { }

        public async Task<ApiResponse> GetMyStrings()
        {
            var uri = BuildUri($"Values/Strings");
            return await GetAsync<ApiResponse>(uri);
        }

        public async Task<ApiResponse> GetMyDto()
        {
            var uri = BuildUri($"Values/Dto");
            return await GetAsync<ApiResponse>(uri);
        }

        public async Task<ApiResponse> GetMyDtos()
        {
            var uri = BuildUri($"Values/Dtos");
            return await GetAsync<ApiResponse>(uri);
        }
        public async Task<ApiResponse> GetMyMasterDto()
        {
            var uri = BuildUri($"Values/MasterDto");
            return await GetAsync<ApiResponse>(uri);
        }
        public async Task<ApiResponse> GetMyException()
        {
            var uri = BuildUri($"Values/Exception");
            return await GetAsync<ApiResponse>(uri);
        }

    }
}
