using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;

using WebApi.Utility;

namespace WebApi
{
    public class ApiResponse
    {
        public string Version { get; set; }
        public string RequestUrl { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public object Content { get; set; }
        public DateTime Timestamp { get; set; }
        public Type ExpectedContentType { get; set; }
        public string ExpectedContentTypeSimplified
        {
            get
            {
                if (ExpectedContentType != null)
                {
                    return ExpectedContentType.GetPrettyName();
                }
                else return null;
            }
        }
        public long? Size { get; set; }

        /// <summary>
        /// Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetTypedContent<T>()
        {
            InvalidCastException exception = new
            InvalidCastException(string.Format("Could not convert Content into {0}.Try using {1}.", typeof(T).Name, Content.GetType().Name));
            if (Content is JObject)
            {
                return (Content as JObject).ToObject<T>();
            }
            else if (Content is JArray)
            {
                return (Content as JArray).ToObject<T>();
            }
            else
            {
                try
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    if (converter != null)
                    {
                        return (T)converter.ConvertFromString(Content.ToString());
                    }
                    throw exception;
                }
                catch (NotSupportedException)
                {
                    throw exception;
                }
            }
        }


    }
}