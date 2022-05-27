using Catalog.Common.Entities;
using Catalog.Common.Interfaces;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CatalogController : ControllerBase
{
	protected readonly IPublishEndpoint _publishEndpoint;
	protected readonly IRequestClient<IUpdatePrice> _requestClient;
	protected readonly IMediator _mediator;

	public CatalogController(IPublishEndpoint publishEndpoint, IRequestClient<IUpdatePrice> requestClient, IMediator mediator)
	{
		_publishEndpoint = publishEndpoint;
		_requestClient = requestClient;
	}

	[HttpGet("SendMessage/{message}")]
	public async Task<ActionResult> SendMessage(string message)
	{
		await _publishEndpoint.Publish<Message>(new
		{
			Text = message
		});

		return Ok();
	}

	[HttpGet("UpdatePrice/{price}")]
	public async Task<ActionResult<decimal>> UpdatePrice(decimal price)
	{
		var response = await _requestClient.GetResponse<IUpdatePrice>(new { price = price });
		return response.Message.price;
	}

	[HttpGet("SendUpdatePrice/{price}")]
	public async Task<ActionResult<decimal>> SendUpdatePrice(decimal price)
	{
		await _mediator.Send<UpdatePrice>(new { price = price });
		return Ok();
	}
}
