using Localization.ServiceAbstraction;
using Localization.Shared;
using Localization.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Localization.Presentation_Api.Controllers;

    [ApiController]
    [Route("api/languages")]
    public class LanguagesController(ILanguageService languages) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<LanguageDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(CancellationToken ct)
        => Ok(await languages.GetActiveAsync(ct));

    [HttpGet("all")]
    [ProducesResponseType(typeof(IReadOnlyList<LanguageDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await languages.GetAllAsync(ct));

    [HttpPost]
    [ProducesResponseType(typeof(LanguageCreatedDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateLanguageRequest request, CancellationToken ct)
    {
        var result = await languages.CreateAsync(request, ct);
        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, result.Value)
            : ToProblem(result);
    }

    [HttpPut("{code}/active")]
    [ProducesResponseType(typeof(LanguageDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> SetActive(string code, [FromBody] SetLanguageActiveRequest request, CancellationToken ct)
    {
        var result = await languages.SetActiveAsync(code, request.IsActive!.Value, ct);
        return result.IsSuccess ? Ok(result.Value) : ToProblem(result);
    }

    [HttpPost("{code}/retranslate")]
    [ProducesResponseType(typeof(TranslationJobDto), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Retranslate(string code, CancellationToken ct)
    {
        var result = await languages.RetranslateAsync(code, ct);
        return result.IsSuccess ? Accepted(result.Value) : ToProblem(result);
    }

    private ObjectResult ToProblem<T>(Result<T> result)
    {
        var status = result.ErrorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unavailable => StatusCodes.Status503ServiceUnavailable,
            _ => StatusCodes.Status500InternalServerError
        };

        return new ObjectResult(new ProblemDetails
        {
            Status = status,
            Title = result.ErrorType.ToString(),
            Detail = result.Error
        })
        { StatusCode = status };
    }
}

