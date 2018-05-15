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
            string expectedKey = "",
                message = "Api Key did not match";
            expectedKey = ConfigurationManager.AppSettings["ClientKey"];
            IEnumerable<string> headerKey = new List<string>();
            if(actionContext.Request.Headers.TryGetValues("ClientKeyHeader", out headerKey))
            {
                if (headerKey.FirstOrDefault() == expectedKey)
                    return true;
                else throw new InvalidOperationException(message);
            }
            throw new InvalidOperationException(message);
        }
    }
}