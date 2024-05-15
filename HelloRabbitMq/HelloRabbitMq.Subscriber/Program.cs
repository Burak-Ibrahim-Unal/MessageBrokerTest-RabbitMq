// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://qjozhkni:2aIy943MANWa6PCklK21ZulOvrqogC0g@roedeer.rmq.cloudamqp.com/qjozhkni");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();
channel.QueueDeclare("hello-queue", true, false, false);

channel.BasicQos(0, 1, false); // false = send x message mean to send messages to every subscriber every turn and divide messages subsscribers count/2 count and share them
var consumer = new EventingBasicConsumer(channel);
channel.BasicConsume("hello-queue", false, consumer);

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Thread.Sleep(1500);
    Console.WriteLine("Message:" + message);

    channel.BasicAck(e.DeliveryTag, false); // true = messages which is processed in memory but are not arrived rabbitmq yet,is informed rabbitmq , false= dont inform...
};

Console.ReadLine();
