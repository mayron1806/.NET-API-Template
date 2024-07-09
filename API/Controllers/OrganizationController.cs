using Application.UseCases.CreateOrganization;
using Application.UseCases.GetOrganizationByUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrganizationController(
    ILogger<OrganizationController> logger,
    ICreateOrganizationUseCase createOrganizationUseCase,
    IGetOrganizationByUserUseCase getOrganizationByUserUseCase
    ) : BaseController(logger)
{
    private readonly IGetOrganizationByUserUseCase _getOrganizationByUserUseCase = getOrganizationByUserUseCase;
    private readonly ICreateOrganizationUseCase _createOrganizationUseCase = createOrganizationUseCase;
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = GetUserId();
        return Ok(await _getOrganizationByUserUseCase.Execute(new() { UserId = userId }));
    }
    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var userId = GetUserId();
        return Ok(await _createOrganizationUseCase.Execute(new() { UserId = userId }));
    }

}
