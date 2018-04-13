using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Configuration;

namespace WebApi.Handlers
{
    public class KeyAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            string expectedKey = "";
            expectedKey = ConfigurationManager.AppSettings["ClientKey"];
            IEnumerable<string> headerKey = new List<string>();
            if(actionContext.Request.Headers.TryGetValues("ClientKeyHeader", out headerKey))
            {
                return headerKey.FirstOrDefault() == expectedKey;
            }
            return false;
        }
    }
}