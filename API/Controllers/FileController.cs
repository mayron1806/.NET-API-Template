using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("api/organization/{organizationId}/[controller]")]
[Authorize]
public class FileController(
    ILogger<FileController> logger
): BaseController(logger)
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = GetUserId();
        var organizationId = GetOrganizationId();
        _logger.LogInformation("[FILE] userId: " + userId + " organizationId: " + organizationId);
        return Ok(new object[] { userId, organizationId });
    }
}
