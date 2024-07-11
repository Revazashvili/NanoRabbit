﻿![NanoRabbit logo](https://raw.githubusercontent.com/cgcel/NanoRabbit/master/Img/logo.png)

[![NuGet](https://img.shields.io/nuget/v/NanoRabbit.svg)](https://nuget.org/packages/NanoRabbit) [![Nuget Downloads](https://img.shields.io/nuget/dt/NanoRabbit)](https://www.nuget.org/packages/NanoRabbit) [![License](https://img.shields.io/github/license/cgcel/NanoRabbit)](https://github.com/cgcel/NanoRabbit)
[![codebeat badge](https://codebeat.co/badges/a37a04d9-dd8e-4177-9b4c-c17526910f7e)](https://codebeat.co/projects/github-com-cgcel-nanorabbit-master)

## About

NanoRabbit, A **Lightweight** RabbitMQ .NET 3rd party library for .NET 6 and up, which makes a simple way to manage *
*Multiple** connections, producers, consumers, and easy to use.

> _NanoRabbit is under development! Please note that some APIs may change their names or usage!_

## Building

| Branch |                                                                                 Building Status                                                                                 |
|:------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------:|
| master | [![build](https://github.com/cgcel/NanoRabbit/actions/workflows/build.yml/badge.svg?branch=master&event=push)](https://github.com/cgcel/NanoRabbit/actions/workflows/build.yml) |
|  dev   |  [![build](https://github.com/cgcel/NanoRabbit/actions/workflows/build.yml/badge.svg?branch=dev&event=push)](https://github.com/cgcel/NanoRabbit/actions/workflows/build.yml)   |

## Features

- Customize the name of producers, consumers.
- Dependency injection available.
- Multiple connections, producers, and consumers can be created.

## Installation

You can get NanoRabbit by grabbing the latest [NuGet](https://www.nuget.org/packages/NanoRabbit) package.

See [Wiki](https://github.com/cgcel/NanoRabbit/wiki/Installation) for more details.

## Version

|                   NanoRabbit                    | RabbitMQ.Client |     .NET      |
|:-----------------------------------------------:|:---------------:|:-------------:|
|    0.0.1, 0.0.2, 0.0.3, 0.0.4, 0.0.5, 0.0.6     |      6.5.0      |      6.0      |
|                      0.0.7                      |      6.5.0      | 6.0, 7.0, 8.0 |
|                  0.0.8, 0.0.9                   |      6.7.0      | 6.0, 7.0, 8.0 |
| 0.1.0, 0.1.1, 0.1.2, 0.1.3, 0.1.4, 0.1.5, 0.1.6 |      6.8.1      | 6.0, 7.0, 8.0 |
|                      0.1.7, 0.1.8               |   6.5.0-6.8.1   | 6.0, 7.0, 8.0 |

## Document

The NanoRabbit Document is at [NanoRabbit Wiki](https://github.com/cgcel/NanoRabbit/wiki).

## QuickStart

> _NanoRabbit is designed as a library depends on **NAMING** Producers, Consumers. So it's important to set
a **UNIQUE NAME** for each Producers, Consumers._

For more, please visit the [Examples](https://github.com/cgcel/NanoRabbit/tree/master/Example).

### Setup RabbitProducers && RabbitConsumers

#### RabbitProducer

Register a RabbitMQ Producer by calling `RabbitHelper()`, and configure it.

```csharp
using NanoRabbit;
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
    } }
});
```

#### RabbitConsumer

Register a RabbitMQ Consumer by calling `RabbitHelper()`, and configure it.

```csharp
using NanoRabbit;
using NanoRabbit.Connection;

var rabbitHelper = new RabbitHelper(rabbitConfig: new RabbitConfiguration
{
    HostName = "localhost",
    Port = 5672,
    VirtualHost = "/",
    UserName = "admin",
    Password = "admin",
    Consumers = new List<ConsumerOptions> { new ConsumerOptions {
            ConsumerName= "FooConsumer",
            QueueName = "foo-queue"
        }
    }
});
```

### Simple Publish

[After](#rabbitproducer) registering a `RabbitProducer` in the `RabbitHelper`, you can simply publish a message by calling `Publish<T>(string producerName, T message)`.

```csharp
rabbitHelper.Publish<string>("FooProducer", "Hello from NanoRabbit");
```

### Simple Consume

[After](#rabbitconsumer) registering a `RabbitConsumer` in the `RabbitConsumer`, you can simply consume a message by
calling `AddConsumer(string consumerName, Action<string> onMessageReceived, int consumers = 1)`.

```csharp
while (true)
{
    rabbitHelper.AddConsumer("FooConsumer", message =>
    {
        Console.WriteLine(message);
    });
}
```

### Forward messages

> Working on it.

### DependencyInjection

#### AddRabbitProducer

```csharp
var builder = Host.CreateApplicationBuilder(args);

// Configure the RabbitMQ Connection
builder.Services.AddRabbitHelper(builder =>
{
    builder.SetHostName("localhost")
        .SetPort(5672)
        .SetVirtualHost("/")
        .SetUserName("admin")
        .SetPassword("admin")
        .AddProducerOption(producer =>
        {
            producer.ProducerName = "FooProducer";
            producer.ExchangeName = "amq.topic";
            producer.RoutingKey = "foo.key";
            producer.Type = ExchangeType.Topic;
        });
});

using IHost host = builder.Build();

host.Run();
```

#### AddRabbitConsumer

```csharp
var builder = Host.CreateApplicationBuilder(args);

// Configure the RabbitMQ Connection
builder.Services.AddRabbitHelper(builder =>
{
    builder.SetHostName("localhost")
        .SetPort(5672)
        .SetVirtualHost("/")
        .SetUserName("admin")
        .SetPassword("admin")
        .AddConsumerOption(consumer =>
        {
            consumer.ConsumerName = "FooConsumer";
            consumer.QueueName = "foo-queue";
        })
        .AddConsumerOption(consumer =>
        {
            consumer.ConsumerName = "BarConsumer";
            consumer.QueueName = "bar-queue";
        });
})
.AddRabbitConsumer<FooQueueHandler>("FooConsumer", consumers: 3)
.AddRabbitConsumer<BarQueueHandler>("BarConsumer", consumers: 2);

using IHost host = builder.Build();

host.Run();
```

More DI Usage at [Wiki](https://github.com/cgcel/NanoRabbit/wiki/DependencyInjection).

## Contributing

1. Fork this repository.
2. Create a new branch in you current repos from the **dev** branch.
3. Push commits and create a Pull Request (PR) to NanoRabbit.

## Todo

- [x] Basic Consume & Publish support
- [x] DependencyInjection support
- [x] Logging support
- [ ] Forward messages
- [ ] ASP.NET support
- [ ] Exchange Configurations
- [x] .NET 7 support
- [x] .NET 8 support
- [ ] Caching of failed sends

## Thanks

- Visual Studio 2022
- [RabbitMQ.Client](https://github.com/rabbitmq/rabbitmq-dotnet-client)
- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
- [Masstransit](https://github.com/masstransit/masstransit)
- [EasyNetQ](https://github.com/autofac/Autofac)

## License

NanoRabbit is licensed under the [MIT](https://github.com/cgcel/NanoRabbit/blob/dev/LICENSE.txt) license.
