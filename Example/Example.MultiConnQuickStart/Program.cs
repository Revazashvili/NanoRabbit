﻿// 发送消息
using NanoRabbit.NanoRabbit;
using RabbitMQ.Client;

var pool = new RabbitPool();
pool.RegisterConnection("Connection1", new ConnectOptions
{
    ConnectConfig = new()
    {
        HostName = "localhost",
        Port = 5672,
        UserName = "admin",
        Password = "admin",
        VirtualHost = "DATA"
    },
    ProducerConfigs = new Dictionary<string, ProducerConfig>
    {
        {
            "DataBasicQueueProducer",
            new ProducerConfig
            {
                ExchangeName = "BASIC.TOPIC",
                RoutingKey = "BASIC.KEY",
                Type = ExchangeType.Topic
            }
        }
    }
});
pool.RegisterConnection("Connection2", new ConnectOptions
{
    ConnectUri = new()
    {
        ConnectionString = "amqp://admin:admin@localhost:5672/HOST"
    },
    ProducerConfigs = new Dictionary<string, ProducerConfig>
    {
        {
            "HostBasicQueueProducer",
            new ProducerConfig
            {
                ExchangeName = "BASIC.TOPIC",
                RoutingKey = "BASIC.KEY",
                Type = ExchangeType.Topic
            }
        }
    }
});

while (true)
{
    pool.SimplePublish("Connection1", "DataBasicQueueProducer", "Hello from conn1");
    await Task.Delay(1000);

    pool.SimplePublish("Connection2", "HostBasicQueueProducer", "Hello from conn2");
    await Task.Delay(1000);
}