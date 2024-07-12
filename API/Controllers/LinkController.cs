using System.Text.Json;
using API.Exceptions;
using Infrastructure.Services.Storage;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LinkController(
    ILogger<LinkController> logger, 
    IUnitOfWork unitOfWork, 
    IStorageService storageService
    ) : BaseController(logger)
{
    private readonly IStorageService _storageService = storageService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    [HttpGet("{transferKey}")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetTransfer([FromRoute] string transferKey)
    {
        _logger.LogInformation("Sending response");
        var transfer = await _unitOfWork.Transfer.GetByKeyWithFiles(transferKey, 20, 0);
        if (transfer == null) return NotFound();
        return Ok(transfer);
    }
    [HttpGet("{transferKey}/files")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetTransferFiles([FromRoute] string transferKey, [FromQuery] int limit = 20, [FromQuery] int offset = 0)
    {
        var files = await _unitOfWork.File.GetListAsync(x => x.Transfer!.Key == transferKey, limit: limit, offset: offset);
        if (files == null) return NotFound();
        return Ok(files);
    }

    [HttpGet("{transferKey}/get-download-url")]
    public async Task DownloadFiles([FromRoute] string transferKey, [FromQuery] bool downloadAll = false, [FromQuery] long? fileId = null)
    {
        const int BATCH_SIZE = 100;
        Response.Headers.Append("Content-Type", "text/event-stream");
        try
        {
            var transfer = await _unitOfWork.Transfer.GetFirstAsync(x => x.Key == transferKey);
            if (transfer == null) 
            {
                await SendErrorAsync(404, "Transferencia nao encontrada");
                return;
            }
            if (downloadAll) {
                var filesCount = await _unitOfWork.File.CountByTransferAsync(transfer.Id);
                if (filesCount == 0) 
                {
                    await SendErrorAsync(404, "Nenhum arquivo encontrado");
                    return;
                }

                for (int i = 0; i < filesCount; i += BATCH_SIZE)
                {
                    var files = await _unitOfWork.File.GetByTransferAsync(transfer.Id, BATCH_SIZE, i);
                    _logger.LogInformation("Transferencia: " + transfer.Id.ToString());
                    _logger.LogInformation("Lendo arquivos: " + i + " - " + (i + BATCH_SIZE).ToString());
                    _logger.LogInformation("Arquivos: " + files.Count().ToString());

                    var tasks = files.Select(file => GetObjectSignedURLAsync(file.Path));
                    try
                    {
                        var urls = await Task.WhenAll(tasks);
                        for (int u = 0; u < urls.Length; u++)
                        {
                            var json = JsonSerializer.Serialize(new { fileKey = files.ToArray()[u].Key, url = urls[u] });
                            await Response.WriteAsync($"data: {json}\n\n");
                            await Response.Body.FlushAsync();
                        }
                    }
                    catch (AggregateException ex)
                    {
                        foreach (var e in ex.InnerExceptions)
                        {
                            if (e is SignURLException) {
                                var json = JsonSerializer.Serialize(new { fileKey = (e as SignURLException)!.FileKey, error = e.Message });
                                await Response.WriteAsync($"data: {json}\n\n");
                                await Response.Body.FlushAsync();
                            }
                        }
                    }

                }
            } else if(fileId != null) {
                try
                {
                    var file = await _unitOfWork.File.GetByIdAsync(fileId.Value);
                    if(file == null) {
                        await SendErrorAsync(404, "Arquivo nao encontrado");
                        return;
                    }
                    var url = await GetObjectSignedURLAsync(fileId.Value.ToString());
                    var json = JsonSerializer.Serialize(new { fileKey = file.Key, url });
                    await Response.WriteAsync($"data: {json}\n\n");
                    await Response.Body.FlushAsync();
                }
                catch (Exception e)
                {
                    if (e is SignURLException) {
                        var json = JsonSerializer.Serialize(new { fileKey = (e as SignURLException)!.FileKey, error = e.Message });
                        await Response.WriteAsync($"data: {json}\n\n");
                        await Response.Body.FlushAsync();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar a transferência de arquivos");
            await SendErrorAsync(500, "Erro interno do servidor");
        }
    }
    private async Task<string> GetObjectSignedURLAsync(string key)
    {
        try
        {
            var res = await _storageService.GetObjectSignedURLAsync(StorageBuckets.FileTransfer, key);
            return res;
        }
        catch (Exception ex)
        {
            throw new SignURLException(key, ex.Message);
        }
    }
    private async Task SendErrorAsync(int statusCode, string message)
    {
        var json = JsonSerializer.Serialize(new { error = message });
        Response.StatusCode = statusCode;
        await Response.WriteAsync($"data: {json}\n\n");
        await Response.Body.FlushAsync();
    }
}