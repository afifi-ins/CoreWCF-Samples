// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CoreWcf.Samples.Pooling
{
    // Service class which implements the service contract interface.
    [CustomLeaseTime(Timeout = 20000)]
    public class WorkService : IWorkService
    {
        public WorkService()
        {
            Thread.Sleep(5000);

            ColorConsole.WriteLine(ConsoleColor.Yellow, "WorkService instance created.");
        }

        public void DoWork()
        {
            ColorConsole.WriteLine(ConsoleColor.Yellow, "WorkService.GetData() completed.");
        }
    }

    // This class takes a lot of time for the initialization, but it has 
    // short method calls.  It uses object pooling.
    [ObjectPooling(MinPoolSize = 0, MaxPoolSize = 5)]
    public class ObjectPooledWorkService : IWorkService
    {
        public ObjectPooledWorkService()
        {
            Thread.Sleep(5000);

            ColorConsole.WriteLine(ConsoleColor.Blue, "ObjectPooledWorkService instance created.");
        }

        public void DoWork()
        {
            ColorConsole.WriteLine(ConsoleColor.Blue, "ObjectPooledWorkService.GetData() completed.");
        }
    }
}
