using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using ApiSample.Data.Entities;
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

        public async Task<ApiResponse> GetDecimal()
        {
            var uri = BuildUri($"Values/Decimal");
            return await GetAsync<ApiResponse>(uri);
        }

        public async Task<ApiResponse> GetStringStringDictionary()
        {
            var uri = BuildUri($"Values/StringStringDictionary");
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
        
        public async Task<ApiResponse> GetPeople(PagedSearchDto dto)
        {
            var uri = BuildUri($"Values/Person/Search");
            return await PostResultAsync<PagedSearchDto>(uri, dto);
        }

    }
}
