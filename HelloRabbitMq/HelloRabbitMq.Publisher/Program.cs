// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Reflection.PortableExecutable;
using System.Text;



var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://qjozhkni:2aIy943MANWa6PCklK21ZulOvrqogC0g@roedeer.rmq.cloudamqp.com/qjozhkni");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

//durable:true = records all messages physically... We dont lose any data if we restart app.AFter messages sent,If there is not ony consumer,messages are lost
channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

Dictionary<string, object> headers = new Dictionary<string, object>(); // key = string, value = object...We select object bcause of we can send image or any other data.
headers.Add("format", "pdf");
headers.Add("shape", "a4");

var basicProperties = channel.CreateBasicProperties();
basicProperties.Headers = headers;
basicProperties.Persistent = true; // for permanent messages...Not delete

channel.BasicPublish("header-exchange", string.Empty, basicProperties, Encoding.UTF8.GetBytes("headers message"));

Console.WriteLine("Message is sent");
Console.ReadLine();

public enum LogNames
{
    Critical = 1, Error = 2, Warning = 3, Info = 4,
}