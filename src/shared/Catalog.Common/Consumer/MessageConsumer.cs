using Catalog.Common.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Catalog.Common.Consumer;
public class MessageConsumer : IConsumer<Message>
{
	readonly ILogger<MessageConsumer> _logger;

	public MessageConsumer(ILogger<MessageConsumer> logger)
	{
		_logger = logger;
	}

	public Task Consume(ConsumeContext<Message> context)
	{
		_logger.LogInformation("Received Text: {Text}", context.Message.Text);

		return Task.CompletedTask;
	}
}

public class MessageConsumerDefinition : ConsumerDefinition<MessageConsumer>
{
	public MessageConsumerDefinition()
	{
		// override the default endpoint name
		EndpointName = "message-service";

		// limit the number of messages consumed concurrently
		// this applies to the consumer only, not the endpoint
		ConcurrentMessageLimit = 8;
	}

	protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<MessageConsumer> consumerConfigurator)
	{
		// configure message retry with millisecond intervals
		endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));

		// use the outbox to prevent duplicate events from being published
		endpointConfigurator.UseInMemoryOutbox();
	}
}
