// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CoreWcf.Samples.Security
{
    // Define a duplex service contract.
    // A duplex contract consists of two interfaces.
    // The primary interface is used to send messages from client to service.
    // The callback interface is used to send messages from service back to client.
    // ICalculatorService allows one to perform multiple operations on a running result.
    // The result is sent back after each operation on the ICalculatorCallback interface.
    [ServiceContract(Namespace = "http://CoreWcf.Samples.Security", SessionMode = SessionMode.Required,
                     CallbackContract = typeof(ICalculatorDuplexCallback))]
    public interface ICalculatorService
    {
        [OperationContract(IsOneWay = true)]
        void Clear();
        [OperationContract(IsOneWay = true)]
        void AddTo(double n);
        [OperationContract(IsOneWay = true)]
        void SubtractFrom(double n);
        [OperationContract(IsOneWay = true)]
        void MultiplyBy(double n);
        [OperationContract(IsOneWay = true)]
        void DivideBy(double n);
    }

    // The callback interface is used to send messages from service back to client.
    // The Result operation will return the current result after each operation.
    // The Equation operation will return the complete equation after Clear() is called.
    public interface ICalculatorDuplexCallback
    {
        [OperationContract(IsOneWay = true)]
        void Result(double result);
        [OperationContract(IsOneWay = true)]
        void Equation(string eqn);
    }
}
