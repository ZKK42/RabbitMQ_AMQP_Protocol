using System.Text;
using System.Text.Json;
using ConsoleApp1;
using RabbitMQ.Client;

var connectionFactory = new ConnectionFactory()
{
    HostName = "127.0.0.1",
    UserName = "guest",
    Password = "guest",
};

using var connection = connectionFactory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "Ogmeter",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
for (int i = 0; i < 10; i++)
{
    Measurement measurement = new Measurement();

    measurement.Id = i;
    measurement.Name = "zkk";
    measurement.Data = 1.7f;

    string logMessageJson = JsonSerializer.Serialize(measurement);

    var body = Encoding.UTF8.GetBytes(logMessageJson);

    channel.BasicPublish(exchange: string.Empty,
                             routingKey: "Ogmeter",
                             basicProperties: null,
                             body: body);
}
Console.WriteLine($" [x] Sent mESSAGES");
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();