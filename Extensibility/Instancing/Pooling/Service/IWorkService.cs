// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CoreWcf.Samples.Pooling
{
    // Define a service contract.
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IWorkService
    {
        [OperationContract]
        void DoWork();
    }
}
