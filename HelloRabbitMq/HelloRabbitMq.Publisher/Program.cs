// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;



var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://qjozhkni:2aIy943MANWa6PCklK21ZulOvrqogC0g@roedeer.rmq.cloudamqp.com/qjozhkni");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

//durable:true = records all messages physically... We dont lose any data if we restart app.AFter messages sent,If there is not ony consumer,messages are lost
channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
            {
                var routeKey = $"route-{x}";
                var queueName = $"direct-queue-{x}";
                channel.QueueDeclare(queueName, true, false, false); //let differenct channels connect
                channel.QueueBind(queueName, "logs-direct", routeKey, null);
            });



Enumerable.Range(1, 50).ToList().ForEach(x =>
            {

                LogNames log = (LogNames)new Random().Next(1, 5);

                string message = $"log-type: {log}";

                var messageBody = Encoding.UTF8.GetBytes(message);

                var routeKey = $"route-{log}";

                channel.BasicPublish("logs-direct", routeKey, null, messageBody);

                Console.WriteLine($"Log is sent {message}");
            });


Console.ReadLine();

public enum LogNames
{
    Critical = 1, Error = 2, Warning = 3, Info = 4,
}