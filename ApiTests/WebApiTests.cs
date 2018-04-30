using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApiClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

using WebApi;
using ApiSample.Data.Entities;

namespace ApiTests
{
    [TestClass]
    public class WebApiTests
    {
        WebApiClient _Client;
        public WebApiTests()
        {
            _Client = new WebApiClient();
        }

        [TestMethod]
        public async Task TestGetStrings()
        {
            ApiResponse data = await _Client.GetMyStrings();

            string[] myData = data.GetTypedContent<string[]>();

            Assert.IsNotNull(data.Content);
        }
        [TestMethod]
        public async Task TestGetDecimal()
        {
            ApiResponse data = await _Client.GetDecimal();
            decimal d = data.GetTypedContent<decimal>();
            Assert.IsTrue(d > 0);
        }
        [TestMethod]
        public async Task TestGetDto()
        {
            ApiResponse data = await _Client.GetMyDto();

            ApiDto dto = data.GetTypedContent<ApiDto>();

            Assert.IsNotNull(dto);
        }

        [TestMethod]
        public async Task TestGetDtos()
        {
            ApiResponse data = await _Client.GetMyDtos();

            List<ApiDto> dto = data.GetTypedContent<List<ApiDto>>();

            Assert.IsNotNull(dto);
        }

        [TestMethod]
        public async Task TestGetMasterDto()
        {
            ApiResponse data = await _Client.GetMyMasterDto();

            MasterApiDto dto = data.GetTypedContent<MasterApiDto>();

            Assert.IsNotNull(dto);
        }

        [TestMethod]
        public async Task MyTestMethod()
        {
            PagedSearchDto dto = new PagedSearchDto();
            dto.PageSize = 25;
            dto.PageNumber = 2;
            dto.OrderByColumn = "FirstName";
            dto.OrderAscending = true;
            dto.TotalRows = 0;
            ApiResponse response = await _Client.GetPeople(dto);

            PagedSearchResponseDto<List<PersonSearchResultDto>> data = response.GetTypedContent<PagedSearchResponseDto<List<PersonSearchResultDto>>>();

            Assert.AreEqual(dto.PageSize, data.Result.Count);

        }

        [TestMethod]
        public async Task TestGetException()
        {
            ApiResponse data = await _Client.GetMyException();

            Assert.IsTrue(!string.IsNullOrEmpty(data.ErrorMessage));
        }
    }
}
