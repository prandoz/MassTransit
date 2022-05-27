using Catalog.Common.Interfaces;
using MassTransit;

namespace Catalog.Common.Consumer;
public class UpdatePriceConsumer : IConsumer<IUpdatePrice>
{
	public async Task Consume(ConsumeContext<IUpdatePrice> context)
	{
		await context.RespondAsync<IUpdatePrice>(new
		{
			price = context.Message.price + 1
		});
	}
}
