// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://qjozhkni:2aIy943MANWa6PCklK21ZulOvrqogC0g@roedeer.rmq.cloudamqp.com/qjozhkni");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

//durable:true = records all messages physically... We dont lose any data if we restart app.AFter messages sent,If there is not ony consumer,messages are lost
//channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout); // not necessary to create it here because we made this exhange at publisher layer

// we will create random queue names per subscribers
//var randomQueueName = //"log-database-save-queue"; //channel.QueueDeclare().QueueName; // 1 queue to feed all subscribers...Locate upper QueueBind to save permanently
var randomQueueName = channel.QueueDeclare().QueueName;

//channel.QueueDeclare(randomQueueName, true, false, false); // 1 queue to feed all subscribers...Locate upper QueueBind to save permanently
channel.QueueBind(randomQueueName, "logs-fanout", "", null);

channel.BasicQos(0, 1, false); // false = send x message mean to send messages to every subscriber every turn and divide messages subsscribers count/2 count and share them
var consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(randomQueueName, false, consumer);

Console.WriteLine("logs-fanout listen process started");

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Thread.Sleep(1500);
    Console.WriteLine("Message:" + message);

    channel.BasicAck(e.DeliveryTag, false); // true = messages which is processed in memory but are not arrived rabbitmq yet,is informed rabbitmq , false= dont inform...
};

Console.ReadLine();
