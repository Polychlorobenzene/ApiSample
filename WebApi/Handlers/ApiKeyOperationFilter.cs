using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace WebApi.Handlers
{
    public class ApiKeyOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            bool showDefaultClientKey = ConfigurationManager.AppSettings["ShowDefaultClientKey"]?.ToLower() == "true";
            if (operation.parameters == null)
            {
                operation.parameters = new List<Parameter>();
            }
            operation.parameters.Add(new Parameter
            {
                name = "ClientKeyHeader",
                @in = "header",
                type = "string",
                required = true,
                @default = showDefaultClientKey ? ConfigurationManager.AppSettings["ClientKey"] : ""
            });
        }
    }
}