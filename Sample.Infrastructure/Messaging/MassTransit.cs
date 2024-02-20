using MassTransit;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInspect.Core.Infrastructure.Messaging
{
    public class MassTransit
    {
        public static IBusControl ConfigureMassTransit() 
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg => 
            {
                cfg.Host(new Uri("rabbitmq://localhost"), h => { /* Configure RabbitMQ settings here */ });
            });
        }
    }
}
