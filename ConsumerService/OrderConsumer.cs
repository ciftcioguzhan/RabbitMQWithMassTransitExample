using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using Microsoft.Extensions.Logging;
using Model;
using System;
using System.Threading.Tasks;
using GreenPipes;
namespace ConsumerService
{
    public class OrderConsumer : IConsumer<Order>
    {
        private readonly ILogger<Order> _logger;
        public OrderConsumer(ILogger<Order> logger)
        {
            _logger = logger;
        }

        public class ClaimSubmissionConsumerDefinition : ConsumerDefinition<OrderConsumer>
        {
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<OrderConsumer> consumerConfigurator)
            {
                endpointConfigurator.UseMessageRetry(r =>
                {
                    r.Handle<Exception>();
                    r.Interval(1, 500);
                });
            }
        }

        public async Task Consume(ConsumeContext<Order> context)
        {

            //if (context.Message.Name != "oguzhan")
            //{
            //    throw new InvalidOperationException("the name error");
            //}

            //1.istek te bunu yap
            if (context.GetRetryAttempt() == 0)
            {
                throw new InvalidOperationException("the name error");

            }
            if (context.GetRetryAttempt() == 1)
            {
                throw new InvalidOperationException("the name error");

            }
            //_logger.LogInformation("isim hataku", context.Message.Name);

            var result = Console.Out.WriteLineAsync(context.Message.Name);
            await result;
        }
    }
}