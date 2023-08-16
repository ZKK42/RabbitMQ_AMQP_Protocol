using System.Text;
using System.Text.Json;
using ConsoleApp2;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, mq) =>
{
    var body = mq.Body.ToArray();
    var logMessageString = Encoding.UTF8.GetString(body);
    Console.WriteLine($"logMessageString message received: {logMessageString}");

    var logMessage = JsonSerializer.Deserialize<Measurement>(logMessageString);

    // Örn: veri tabanına ekle
    // Örn: dosyaya kaydet
    Console.WriteLine($"logMessage received: {logMessage}");

};

channel.BasicConsume(queue: "Ogmeter",
                     autoAck: true, //true ise mesaj otomatik olarak kuyruktan silinir
                     consumer: consumer);
Console.ReadLine();