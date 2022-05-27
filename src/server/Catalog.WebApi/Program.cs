using System.Reflection;
using Catalog.Common.Interfaces;
using Catalog.Common.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region MASSTRANSIT

builder.Services.AddMassTransit(x =>
{
	x.UsingRabbitMq((context, cfg) =>
	{

		cfg.Host("localhost", "/", h =>
		{
			h.Username("root");
			h.Password("root");
		});

		cfg.ConfigureEndpoints(context);
	});
});

builder.Services.AddHostedService<Worker>();

builder.Services.AddMediator(cfg =>
{
	cfg.AddConsumers(Assembly.GetAssembly(typeof(Catalog.Common.Consumer.UpdatePriceConsumer)));
	cfg.AddRequestClient<IUpdatePrice>();
});

// OPTIONAL, but can be used to configure the bus options
//builder.Services.AddOptions<MassTransitHostOptions>()
//	.Configure(options =>
//	{
//		// if specified, waits until the bus is started before
//		// returning from IHostedService.StartAsync
//		// default is false
//		options.WaitUntilStarted = true;

//		// if specified, limits the wait time when starting the bus
//		options.StartTimeout = TimeSpan.FromSeconds(10);

//		// if specified, limits the wait time when stopping the bus
//		options.StopTimeout = TimeSpan.FromSeconds(30);
//	});

#endregion MASSTRANSIT

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
