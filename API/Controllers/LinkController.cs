using System.Net;
using System.Text.Json;
using API.Exceptions;
using Application.Utils;
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
    public async Task<IActionResult> DownloadFiles(
        [FromRoute] string transferKey, 
        [FromQuery] bool? downloadAll = false, 
        [FromQuery] long? fileId = null,
        [FromQuery] string? password = null
        )
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        try
        {
            if (downloadAll == null && fileId == null) return await SendErrorAsync(400, "É necessário informar os arquivos para download");
            var transfer = await _unitOfWork.Transfer.GetFirstAsync(x => x.Key == transferKey);
            if (transfer == null) return await SendErrorAsync(404, "Arquivos não encontrados");
            if (transfer.Expired) return await SendErrorAsync(404, "Arquivos expirados");
            if (transfer.Send?.Password != null)
            {
                var verified = Security.VerifyPassword(password ?? "", transfer.Send.Password);
                if (!verified) return await SendErrorAsync(401, "Senha inválida");
            }
            
            var response = downloadAll!.Value ? await HandleDownloadAllFiles(transfer.Id) : await HandleDownloadSingleFile(fileId!.Value);
            // add contador de download
            transfer.Send!.IncrementDownload();
            if (transfer.Send.ExpiresOnDowload) transfer.SetAsExpired();
            _unitOfWork.Transfer.Update(transfer);
            await _unitOfWork.SaveChangesAsync();
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar a transferência de arquivos");
            return await SendErrorAsync(500, "Erro interno do servidor");
        }
    }
    private async Task<IActionResult> HandleDownloadAllFiles(int transferId) {
        const int BATCH_SIZE = 500;
        var filesCount = await _unitOfWork.File.CountByTransferAsync(transferId);
        if (filesCount == 0) return await SendErrorAsync(404, "Nenhum arquivo encontrado");
        try
        {
            for (int i = 0; i < filesCount; i += BATCH_SIZE)
            {
                var files = await _unitOfWork.File.GetByTransferAsync(transferId, BATCH_SIZE, i);
                _logger.LogInformation("Transferencia: " + transferId.ToString());
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
            return Ok();
        }
        catch (Exception)
        {
            return await SendErrorAsync(500, "Erro interno do servidor");
        }
        
    }
    private async Task<IActionResult> HandleDownloadSingleFile(long fileId)
    {
        try
        {
            var file = await _unitOfWork.File.GetByIdAsync(fileId);
            if (file == null) return await SendErrorAsync(404, "Arquivo nao encontrado");
            var url = await GetObjectSignedURLAsync(file.Key);
            var json = JsonSerializer.Serialize(new { fileKey = file.Key, url });
            await Response.WriteAsync($"data: {json}\n\n");
            await Response.Body.FlushAsync();
            return Ok();
        }
        catch (Exception e)
        {
            if (e is SignURLException) {
                var json = JsonSerializer.Serialize(new { fileKey = (e as SignURLException)!.FileKey, error = e.Message });
                await Response.WriteAsync($"data: {json}\n\n");
                await Response.Body.FlushAsync();
            }
            return await SendErrorAsync(500, "Erro interno do servidor");
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
    private async Task<IActionResult> SendErrorAsync(int statusCode, string message)
    {
        var json = JsonSerializer.Serialize(new { error = message });
        Response.StatusCode = statusCode;
        await Response.WriteAsync($"data: {json}\n\n");
        await Response.Body.FlushAsync();
        return new StatusCodeResult(statusCode);
    }
}