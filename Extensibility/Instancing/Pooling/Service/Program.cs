// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

const int HttpPort = 5000;
const int NetTcpWorkServicePort = 8000;
const int NetTcpObjectPooledWorkServicePort = 8001;

var builder = WebApplication.CreateBuilder();

builder.WebHost
.UseKestrel(options =>
{
    options.ListenAnyIP(HttpPort);
})
.UseNetTcp(NetTcpWorkServicePort)
.UseNetTcp(NetTcpObjectPooledWorkServicePort);

//Enable CoreWCF Services, with metadata (WSDL) support
builder.Services.AddServiceModelServices()
    .AddServiceModelMetadata()
    .AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>()
    .AddSingleton<WorkService>();

var app = builder.Build();

app.UseServiceModel(builder =>
{
    // Add the Calculator Service
    builder.AddService<WorkService>(serviceOptions =>
    {
        serviceOptions.BaseAddresses.Clear();
        // Set the default host name:port in generated WSDL and the base path for the address 
        serviceOptions.BaseAddresses.Add(new Uri("http://localhost/EchoService"));
        serviceOptions.BaseAddresses.Add(new Uri($"net.tcp://localhost:{NetTcpWorkServicePort}/WorkService"));
        serviceOptions.BaseAddresses.Add(new Uri($"net.tcp://localhost:{NetTcpObjectPooledWorkServicePort}/WorkService"));
    })
    // Add NetTcpBinding endpoint
    .AddServiceEndpoint<WorkService, IWorkService>(new NetTcpBinding(SecurityMode.None), "netTcp");

    // Configure WSDL to be available
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpGetEnabled = true;
});

PrintLegend();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Echo service started.");
Console.ForegroundColor = ConsoleColor.Gray;

app.Run();

static void PrintLegend()
{
    Console.WriteLine("=================================");

    ColorConsole.WriteLine(ConsoleColor.Green, "Green: Messages from the ServiceHost.");

    ColorConsole.WriteLine(ConsoleColor.Yellow, "Yellow: Messages from WorkService instance.");

    ColorConsole.WriteLine(ConsoleColor.Blue, "Blue: Messages from ObjectPooledWorkService instance.");

    ColorConsole.WriteLine(ConsoleColor.Red, "Red: Messages from the object pool.");

    Console.WriteLine("=================================");
    Console.WriteLine();
}

internal static class ColorConsole
{
    public static void WriteLine(ConsoleColor color, string text, params object[] args)
    {
        ConsoleColor currentColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text, args);
        Console.ForegroundColor = currentColor;
    }
}
