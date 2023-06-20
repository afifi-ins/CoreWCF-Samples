// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System.ServiceModel;
using System.ServiceModel.Security;

//The service contract is defined using Connected Service "WCF Web Service", generated from the service by the dotnet svcutil tool.

// Construct InstanceContext to handle messages on callback interface
InstanceContext site = new InstanceContext(new CallbackHandler());

CalculatorServiceClient client = new CalculatorServiceClient(site);
client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.PeerOrChainTrust;

Console.WriteLine("Press <ENTER> to terminate client once the output is displayed.");
Console.WriteLine();

// Call the AddTo service operation.
double value = 100.00D;
client.AddTo(value);

// Call the SubtractFrom service operation.
value = 50.00D;
client.SubtractFrom(value);

// Call the MultiplyBy service operation.
value = 17.65D;
client.MultiplyBy(value);

// Call the DivideBy service operation.
value = 2.00D;
client.DivideBy(value);

// Complete equation
client.Clear();

Console.ReadLine();

//Closing the client gracefully closes the connection and cleans up resources
client.Close();

Console.WriteLine();
Console.WriteLine("Press <ENTER> to terminate client.");
Console.ReadLine();

public interface ICalculatorDuplexCallback
{
    [OperationContract(IsOneWay = true)]
    void Result(double result);
    [OperationContract(IsOneWay = true)]
    void Equation(string eqn);
}

// Define class which implements callback interface of duplex contract
public class CallbackHandler : ICalculatorDuplexCallback
{
    public void Result(double result)
    {
        Console.WriteLine("Result({0})", result);
    }

    public void Equation(string eqn)
    {
        Console.WriteLine("Equation({0})", eqn);
    }
}
