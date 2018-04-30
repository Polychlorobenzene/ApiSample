using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using WebApi.Handlers;
using ApiSample.Data.Repositories;
using ApiSample.Data.Entities;

namespace WebApi.Controllers
{
    [KeyAuthorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet, Route("api/Values/Decimal")]
        public IHttpActionResult GetDecimal ()
        {
            decimal d = 1.25m;
            return Ok(d);
        }
        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
        [Route("api/Values/Strings")]
        public IHttpActionResult GetStrings()
        {
            return Ok(new string[] { "1", "2", "3" });
        }

        [Route("api/Values/Dto")]
        public IHttpActionResult GetDto()
        {
            ApiDto dto = new ApiDto()
            {
                Id = 12,
                Name = "ApiDto",
                ParentId = 22
            };
            return Ok(dto);
        }

        [Route("api/Values/Dtos")]
        public IHttpActionResult GetDtos()
        {
            ApiDto dto1 = new ApiDto()
            {
                Id = 12,
                Name = "ApiDto1",
                ParentId = 13
            };
            ApiDto dto2 = new ApiDto()
            {
                Id = 13,
                Name = "ApiDto2",
                ParentId = 22
            };
            return Ok(new List<ApiDto>() { dto1, dto2 });
        }

        [Route("api/Values/MasterDto")]
        public IHttpActionResult GetMasterDtos()
        {
            MasterApiDto m1 = new MasterApiDto()
            {
                Id = 1,
                ParentId = null,
                Workers = new List<ApiDto>()
                {
                    new ApiDto()
                    {
                        Id = 12,
                        MasterId = 1,
                        Name = "ApiDto1",
                        ParentId = 13
                    },
                   new ApiDto()
                    {
                        Id = 13,
                        MasterId = 1,
                        Name = "ApiDto2",
                        ParentId = null
                    }
               },
                Children = new List<MasterApiDto>()
                {
                    new MasterApiDto()
                    {
                        Id=2,
                        ParentId = 1
                    }
                }
            };

            return Ok(m1);
        }

        [HttpPost, Route("api/Values/Person/Search")]
        public IHttpActionResult SearchPeople(PagedSearchDto dto)
        {
            using (ApiSampleRepository rep = new ApiSampleRepository())
            {
                var results = rep.SearchPeople(dto);
                return Ok(results);
            }
        }

        [Route("api/Values/Exception")]
        public IHttpActionResult GetException()
        {
            try
            {
                throw new InvalidOperationException("Test Message Outer", new InvalidOperationException("Test Message Inner"));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
