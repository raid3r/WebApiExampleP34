using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApiExampleP34.Application.Swagger;

public class AuthorizeCheckOperationFilter: IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Проверяем наличие [Authorize] на методе или на контроллере
        var hasAuthorize = context.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() == true
                           || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

        // Если нет [Authorize], но есть [AllowAnonymous] — явно не добавляем
        var hasAllowAnonymous = context.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any() == true
                                || context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();

        if (hasAuthorize && !hasAllowAnonymous)
        {
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { } // если нужны scopes — укажите их здесь
                    }
                }
            };
        }
    }
}
