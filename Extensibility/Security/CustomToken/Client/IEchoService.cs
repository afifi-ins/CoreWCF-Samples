// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ServiceModel;

namespace CoreWcf.Samples.CustomToken
{
    [ServiceContract]
    public interface IEchoService : IDisposable
    {
        [OperationContract]
        string Echo();
    }
}
