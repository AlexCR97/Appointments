using Appointments.Api.Assets.Extensions.Files;
using Appointments.Api.Assets.Models;
using Appointments.Api.Core.User;
using Appointments.Assets.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Assets.Controllers;

[ApiController]
[Route("api/assets")]
[ApiVersion("1.0")]
[Produces("application/json")]
[Authorize(Policy = AssetsApiPolicy.Assets.Scope)]
public class AssetsController : ControllerBase
{
    private readonly ISender _sender;

    public AssetsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost(Name = nameof(CreateAsset))]
    public async Task<AssetCreatedResponse> CreateAsset([FromBody] CreateAssetRequest request)
    {
        var result = await _sender.Send(request.ToApplicationRequest(
            User.GetAccessToken().Username));

        return new AssetCreatedResponse(
            result.Id,
            result.TransactionCode);
    }

    [HttpGet("{id}", Name = nameof(GetAsset))]
    public async Task<AssetResponse> GetAsset([FromRoute] Guid id)
    {
        var asset = await _sender.Send(new Appointments.Assets.Application.GetAssetRequest(id));
        return AssetResponse.From(asset);
    }

    [HttpPost("blob/{path}", Name = nameof(UploadAsset))]
    public async Task<AssetUploadResponse> UploadAsset(
        [FromRoute] string path,
        [FromForm] IFormFile file,
        [FromHeader(Name = "X-TransactionCode")] string transactionCode)
    {
        var transactionStatus = await _sender.Send(new Appointments.Assets.Application.UploadAssetRequest(
            new AssetPath(path),
            file.GetBytes(),
            transactionCode));

        return new AssetUploadResponse(transactionStatus.ToString());
    }

    [HttpGet("blob/{path}", Name = nameof(ServeAsset))]
    public async Task<FileContentResult> ServeAsset([FromRoute] string path)
    {
        var asset = await _sender.Send(new Appointments.Assets.Application.GetAssetByPathRequest(new AssetPath(path)));
        var assetContents = await _sender.Send(new Appointments.Assets.Application.GetAssetContentsRequest(asset.Id));
        return File(assetContents, asset.ContentType);
    }

    [HttpDelete("blob/{path}", Name = nameof(DeleteAsset))]
    public async Task<NoContentResult> DeleteAsset([FromRoute] string path)
    {
        Asset asset = await _sender.Send(new Appointments.Assets.Application.GetAssetByPathRequest(new AssetPath(path)));

        await _sender.Send(new Appointments.Assets.Application.DeleteAssetRequest(
            User.GetAccessToken().Username,
            asset.Id));

        return NoContent();
    }
}
