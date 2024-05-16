// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://qjozhkni:2aIy943MANWa6PCklK21ZulOvrqogC0g@roedeer.rmq.cloudamqp.com/qjozhkni");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.BasicQos(0, 2, false);// false = send x message mean to send messages to every subscriber every turn and divide messages subsscribers count/2 count and share them
var consumer = new EventingBasicConsumer(channel);

var queueName = "direct-queue-Warning";
channel.BasicConsume(queueName, false, consumer);

Console.WriteLine("logs-direct is listening");

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
          {
              var message = Encoding.UTF8.GetString(e.Body.ToArray());

              Thread.Sleep(1500);
              Console.WriteLine("Message:" + message);

              //File.AppendAllText("log-critical.txt",message + "\n");

              channel.BasicAck(e.DeliveryTag, false);// true = messages which is processed in memory but are not arrived rabbitmq yet,is informed rabbitmq , false= dont inform...
          };

Console.ReadLine();
