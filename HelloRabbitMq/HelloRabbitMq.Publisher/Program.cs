﻿// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;



var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://qjozhkni:2aIy943MANWa6PCklK21ZulOvrqogC0g@roedeer.rmq.cloudamqp.com/qjozhkni");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

//durable:true = records all messages physically... We dont lose any data if we restart app.AFter messages sent,If there is not ony consumer,messages are lost
channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic, autoDelete: false);

Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                LogNames log1 = (LogNames)new Random().Next(1, 5);
                LogNames log2 = (LogNames)new Random().Next(1, 5);
                LogNames log3 = (LogNames)new Random().Next(1, 5);

                var routeKey = $"{log1}.{log2}.{log3}";
                string message = $"log-type: {log1}-{log2}-{log3}";
                var messageBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("logs-topic", routeKey, null, messageBody);

                Console.WriteLine($"Log is sent {message}");
            });


Console.ReadLine();

public enum LogNames
{
    Critical = 1, Error = 2, Warning = 3, Info = 4,
}