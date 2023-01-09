using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;

// Configure the connection and define hubConnection.On<> to show incoming messages
var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5086/hubs/NotificationHub")
                .AddMessagePackProtocol()
                .ConfigureLogging(x =>
                {
                    x.AddConsole();
                    x.SetMinimumLevel(LogLevel.Error);
                })
                .Build();

connection.On<string, string>("ShowMessage", (username, message) =>
{
    Console.WriteLine($"{username}: {message}");
});

// Start the connection
await connection.StartAsync();

// Read the username from the console
Console.WriteLine("Enter your username: ");
var username = Console.ReadLine();

// Enter a loop to continuously read messages from the console and send them to the server
while (true)
{
    var message = Console.ReadLine();
    await connection.InvokeAsync("SendMessageToAllClients", username, message);
}
