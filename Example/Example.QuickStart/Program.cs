﻿using NanoRabbit;
using NanoRabbit.Connection;

var rabbitHelper = new RabbitHelper(rabbitConfig: new RabbitConfiguration
{
    HostName = "localhost",
    Port = 5672,
    VirtualHost = "/",
    UserName = "admin",
    Password = "admin",
    Producers = new List<ProducerOptions> { new ProducerOptions {
            ProducerName = "FooProducer",
            ExchangeName = "amq.topic",
            RoutingKey = "foo.key"
        }
    },
    Consumers = new List<ConsumerOptions> { new ConsumerOptions {
            ConsumerName= "FooConsumer",
            QueueName = "foo-queue"
        }
    }
});

rabbitHelper.Publish<string>("FooProducer", "Hello from NanoRabbit");

Console.WriteLine(" Press [enter] to exit.");

while (true)
{
    rabbitHelper.AddConsumer("FooConsumer", message =>
    {
        Console.WriteLine(message);
    });

    if (Console.ReadLine() == "")
    {
        break;
    }
}
