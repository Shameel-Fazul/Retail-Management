using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace RMDataManager.App_Start
{
    public class AuthorizationOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
            {
                operation.parameters = new List<Parameter>();
            }

            operation.parameters.Add(new Parameter
            {
                name = "Authorization",
                @in = "header",
                description = "access token",
                required = false,
                type = "string",
                @default = "bearer adLk57GGSPLSwmZJuoQx9Rsk75qbCRYSz1SfMP2ha-PGrYy3JCB44GddtaUtpWjbFyjHX0ngl4r1eMirOKET_S71AaP65PT1tPHXlyrvnFM1LGEJYnN5U7LHZW0rO4kHUYXI6oSHnGO2UruHUUvAEjf9ON0YH7k3OUoQmftx-dG_qXxYWeyZ0ZDk9HsbcegvKlyN3RgxIxdfoiUPZgwbK_gC-ikE1GhUylHliDxN6hHpob6s6k9dB5uv1QxH2ZT6OzcTgfTfHY9nqfFfwjbigSvNZ0ckYgBhdLwC5oNAwtgzw6IVOIkrmX_jgIb-mY22GDKXdC7SXaxu1yJchQ7bVlZVEvt5Xj1Qit9vBmILVKJMDlfA9WLcLwh7idVXIz_r2Z1oAQUgY6p1WmXftPoi5-Fu_s-fXY0-EoQXgikyAiDGqEayCW0cutuN8qFWDM240G1pOn7-YeDbu1iGtxmUEsP9HD-Jmvv5QNn1fw8LVafoOv62SPkmWre0g-Rr3bjF"
            });
        }
    }
}