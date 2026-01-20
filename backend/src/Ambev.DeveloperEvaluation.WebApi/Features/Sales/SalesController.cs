using Ambev.DeveloperEvaluation.Application.Read.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.Application.Read.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Common.Pagination;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

[ApiController]
[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of SalessController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public SalesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new sale
    /// </summary>
    /// <param name="request">The sale creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateSaleCommand>(request);

        var response = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
        {
            Success = true,
            Message = "Sale created successfully",
            Data = _mapper.Map<CreateSaleResponse>(response)
        });
    }

    /// <summary>
    /// Cancel an existing sale
    /// </summary>
    /// <param name="request">The sale cancellation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <remarks>
    /// This operation changes the sale status to <see cref="SaleStatus.Cancelled"/>
    /// and may trigger domain events.
    /// </remarks>
    [HttpDelete("{Id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CancelSale([FromRoute] CancelSaleRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CancelSaleCommand>(request);

        await _mediator.Send(command, cancellationToken);

        return Ok("Sale cancelled successfully");
    }

    /// <summary>
    /// Update an existing sale and items
    /// </summary>
    /// <param name="id">The sale id</param>
    /// <param name="request">The sale update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateSale([FromRoute] Guid id, [FromBody] UpdateSaleRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateSaleCommand>(request);
        command.Id = id;

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Query all sales paginated
    /// </summary>
    /// <param name="request">The get sales request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<GetAllSalesResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSales([FromQuery] GetAllSalesRequest request,
        CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetAllSalesQuery>(request);

        var result = await _mediator.Send(query, cancellationToken);

        var response = new PaginatedList<GetAllSalesResponse>(_mapper.Map<List<GetAllSalesResponse>>(result.Data),
            result.TotalItems, result.CurrentPage, result.PageSize);

        return OkPaginated(response);
    }

    /// <summary>
    /// Query sale by unique identifier
    /// </summary>
    /// <param name="request">The get sale request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("{Id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSale([FromRoute] GetSaleRequest request,
        CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetSaleQuery>(request);

        var result = await _mediator.Send(query, cancellationToken);

        return Ok(_mapper.Map<GetSaleResponse>(result));
    }
}