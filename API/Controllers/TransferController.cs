using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/organization/{organizationId}/[controller]")]
    [Authorize]
    public class TransferController(ILogger<TransferController> logger, IUnitOfWork unitOfWork) : BaseController(logger)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [HttpGet]
        public async Task<IActionResult> GetTransfer([FromQuery] int limit = 20, [FromQuery] int offset = 0)
        {
            var organizationId = GetOrganizationId();
            var transfer = await _unitOfWork.Transfer.GetListAsync(x => x.OrganizationId == organizationId, limit: limit, offset: offset);
            if (transfer == null) return NotFound();
            _logger.LogInformation("Sending response");
            return Ok(transfer);
        }
        [HttpGet("{transferKey}")]
        public async Task<IActionResult> GetTransfer([FromRoute] string transferKey)
        {
            var transfer = await _unitOfWork.Transfer.GetByKeyWithFiles(transferKey, 20, 0);
            if (transfer == null) return NotFound();
            _logger.LogInformation("Sending response");
            return Ok(transfer);
        }

        [HttpGet("{transferKey}/files")]
        public async Task<IActionResult> GetTransferFiles([FromRoute] string transferKey, [FromQuery] int limit = 20, [FromQuery] int offset = 0)
        {
            var files = await _unitOfWork.File.GetListAsync(x => x.Transfer!.Key == transferKey, limit: limit, offset: offset);
            if (files == null) return NotFound();
            return Ok(files);
        }
    }
}