using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ScheduleApi.Security;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    private const string AdminApiKeyHeaderName = "X-Admin-Api-Key";

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        
        var expectedAdminApiKey = configuration.GetValue<string>("AdminSettings:ApiKey");
        
        if (!context.HttpContext.Request.Headers.TryGetValue(AdminApiKeyHeaderName, out var potentialAdminApiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        if (!expectedAdminApiKey.Equals(potentialAdminApiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}