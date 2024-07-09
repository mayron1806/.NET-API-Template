using System.Security.Claims;
using Application.UseCases.GetOrganizationByUser;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace API.Middlewares;

public class OrganizationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine("[ORGANIZATION MIDDLEWARE]");
        var organizationId = context.Request.RouteValues["organizationId"]?.ToString();
        if (!string.IsNullOrEmpty(organizationId)) {
            var logger = context.RequestServices.GetRequiredService<ILogger<OrganizationMiddleware>>();
            var userId = context.User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                await WriteForbiddenResponse(context, "Usuário não tem autorizacão para essa organização");
                return;
            }
            // pega ou cria organização no cache
            var memoryCache = context.RequestServices.GetRequiredService<IMemoryCache>();
            var organization = await memoryCache.GetOrCreateAsync(
                "organization_" + organizationId + "_user_" + userId,
                async entry => {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);
                    var getOrganizationByUserUseCase = context.RequestServices.GetRequiredService<IGetOrganizationByUserUseCase>();
                    var org = await getOrganizationByUserUseCase.Execute(new() { UserId = int.Parse(userId) });
                    logger.LogInformation("[MISS] Organization: " + JsonConvert.SerializeObject(org));
                    return org;
                }
            );

            if (organization == null || organization.OrganizationId.ToString() != organizationId)
            {
                await WriteForbiddenResponse(context, "Usuário não tem autorizacão para essa organização");
                return;
            }
        }
        context.Items["organizationId"] = organizationId;
        await _next(context);
    }
    private async Task WriteForbiddenResponse(HttpContext context, string message)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        context.Response.ContentType = "application/json";
        var response = new { Message = message };
        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
    }
}
