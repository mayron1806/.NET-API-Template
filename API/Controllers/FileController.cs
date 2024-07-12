using Application.UseCases.ConfirmFilesUpload;
using Application.UseCases.PrepareFilesUpload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("api/organization/{organizationId}/[controller]")]
public class FileController(
    ILogger<FileController> logger,
    IPrepareFilesUpload prepareFilesUpload,
    IConfirmFilesUpload confirmFilesUpload
): BaseController(logger)
{
    private readonly IPrepareFilesUpload _prepareFilesUpload = prepareFilesUpload;
    private readonly IConfirmFilesUpload _confirmFilesUpload = confirmFilesUpload;
    [HttpPost]
    public async Task<ActionResult<PrepareFilesUploadInputDto>> PrepareUpload([FromBody] PrepareFilesUploadInputDto body) 
    {
        var userId = GetUserId();
        var organizationId = GetOrganizationId();
        return Ok(await _prepareFilesUpload.Execute(new() { 
            UserId = userId,
            OrganizationId = organizationId,
            Files = body.Files,
            EmailsDestination = body.EmailsDestination,
            ExpiresAt = body.ExpiresAt,
            ExpiresOnDownload = body.ExpiresOnDownload,
            Message = body.Message,
            Password = body.Password, 
            QuickDownload = body.QuickDownload
        }));
    }
    [HttpPost("confirm")]
    [AllowAnonymous]
    public async Task<IActionResult> Confirm([FromBody] ConfirmFilesUploadInputDto body) => Ok(await _confirmFilesUpload.Execute(body));
}
