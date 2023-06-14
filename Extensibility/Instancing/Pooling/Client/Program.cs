// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;

Console.WriteLine("Press <ENTER> to start the client.");
Console.ReadLine();

CallWorkService();
CallObjectPooledWorkService();

Console.WriteLine("Press <ENTER> to exit.");
Console.ReadLine();

static void CallWorkService()
{
    NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
    var endpointAddress = new EndpointAddress("net.tcp://localhost:8000/WorkService/netTcp");
    ChannelFactory<IWorkService> channelFactory = new ChannelFactory<IWorkService>(binding, endpointAddress);

    IWorkService channel = channelFactory.CreateChannel();

    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();

    ColorConsole.WriteLine(ConsoleColor.Yellow, "Calling WorkService:");

    // Call the service method for 5 times
    for (int i = 1; i <= 5; i++)
    {
        channel.DoWork();
        ColorConsole.WriteLine(ConsoleColor.Yellow, "{0} - DoWork() Done", i);
    }

    stopwatch.Stop();
    ColorConsole.WriteLine(ConsoleColor.Yellow, "Calling WorkService took: " + stopwatch.ElapsedMilliseconds.ToString() + " ms.");

    ((IClientChannel)channel).Close();
}

static void CallObjectPooledWorkService()
{
    NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
    var endpointAddress = new EndpointAddress("net.tcp://localhost:8001/WorkService/netTcp");
    ChannelFactory<IWorkService> channelFactory = new ChannelFactory<IWorkService>(binding, endpointAddress);

    IWorkService channel = channelFactory.CreateChannel();

    ColorConsole.WriteLine(ConsoleColor.Blue, "Calling ObjectPooledWorkService:");

    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();

    // Call the service method for 5 times
    for (int i = 1; i <= 5; i++)
    {
        channel.DoWork();
        ColorConsole.WriteLine(ConsoleColor.Blue, "{0} - DoWork() Done", i);
    }

    stopwatch.Stop();
    ColorConsole.WriteLine(ConsoleColor.Blue, "Calling ObjectPooledWorkService took: " + stopwatch.ElapsedMilliseconds.ToString() + " ms.");

    ((IClientChannel)channel).Close();
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

[ServiceContract]
interface IWorkService
{
    [OperationContract]
    void DoWork();
}
