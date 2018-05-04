using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

using WebApi;

namespace WebApi.Handlers
{
    public class ApiResponseHandler : DelegatingHandler
    {
        //Original source: https://www.infoworld.com/article/3207541/application-development/how-to-make-your-asp-net-web-api-responses-consistent-and-useful.html by Joydip Kanjilal
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            if (request.RequestUri.OriginalString.ToLower().Contains("swagger"))
            {
                return response;
            }
            try
            {
                return GenerateResponse(request, response);
            }
            catch (Exception ex)
            {
                return request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        private HttpResponseMessage GenerateResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            string errorMessage = null;
            HttpStatusCode statusCode = response.StatusCode;
            if (!IsResponseValid(response))
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Request");
            }
            //From Http 1.1 Protocol: The 204 response MUST NOT include a message-body, and thus is always terminated by the first empty line after the header fields.
            //https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html#sec10.2.5
            if (statusCode == HttpStatusCode.NoContent)
            {                                
                return request.CreateResponse(statusCode);
            }
            object responseContent;
            if (response.TryGetContentValue(out responseContent))
            {
                HttpError httpError = responseContent as HttpError;
                if (httpError != null)
                {
                    errorMessage = httpError.Message;
                    statusCode = HttpStatusCode.InternalServerError;
                    responseContent = null;
                }
            }
            ApiResponse apiResponse = new ApiResponse();
            apiResponse.ExpectedContentType = responseContent.GetType();
            apiResponse.Version = "1.0";
            apiResponse.StatusCode = statusCode;
            apiResponse.Content = responseContent;
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            apiResponse.Timestamp = dt;
            apiResponse.ErrorMessage = errorMessage;
            apiResponse.Size = responseContent.ToString().Length;
            var result = request.CreateResponse(response.StatusCode, apiResponse);
            return result;
        }
        private bool IsResponseValid(HttpResponseMessage response)
        {
            if ((response != null))
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Accepted:
                    case HttpStatusCode.Continue:
                    case HttpStatusCode.Created:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.NonAuthoritativeInformation:
                    case HttpStatusCode.OK:
                    case HttpStatusCode.PartialContent:
                    case HttpStatusCode.ResetContent:
                        return true;
                    default:
                        return false;
                }
            }
            return false;
        }
    }
}