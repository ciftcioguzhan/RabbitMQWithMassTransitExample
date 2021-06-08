using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenPipes;
using static ConsumerService.OrderConsumer;

namespace ConsumerService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMassTransit(x =>
            {
                 //tekrar deneme 2.yol
                //x.AddConsumer<OrderConsumer, ClaimSubmissionConsumerDefinition>();

                x.AddConsumer<OrderConsumer>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    // configure health checks for this bus instance
                    cfg.UseHealthCheck(provider);

                    cfg.Host("rabbitmq://localhost");

                    cfg.ReceiveEndpoint("order-queue", ep =>
                    {

                        // tekrar deneme 1.yol
                        //ep.UseMessageRetry(r => r.Interval(2, 500));

                        //ep.UseMessageRetry(r => r.Immediate(5));

                        ep.ConfigureConsumer<OrderConsumer>(provider);
                    });

                    //ınter 3.yol
                    cfg.ReceiveEndpoint("order-queue", ep =>
                    {

                        ep.ConfigureConsumer<OrderConsumer>(provider);
                    });

                    //ınter 3.yol
                    cfg.ReceiveEndpoint("order-queue", ep =>
                    {

                        ep.ConfigureConsumer<OrderConsumer>(provider);
                    });
                }));
            });

            services.AddMassTransitHostedService();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
