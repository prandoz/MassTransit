using Catalog.Common.Entities;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CatalogController : ControllerBase
{
	protected readonly IPublishEndpoint _publishEndpoint;

	public CatalogController(IPublishEndpoint publishEndpoint)
	{
		_publishEndpoint = publishEndpoint;
	}

	[HttpGet("{price}")]
	public async Task<ActionResult> UpdatePrice(decimal price)
	{
		await _publishEndpoint.Publish<Message>(new
		{
			Text = "Ciao"
		});

		return Ok();
	}
}
