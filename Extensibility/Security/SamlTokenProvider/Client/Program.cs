// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Federation;
using Microsoft.IdentityModel.Tokens.Saml2;

//The service contract is defined using Connected Service "WCF Web Service", generated from the service by the dotnet svcutil tool.

// Call the two different endpoints
CallEndpoint("SymmetricKey");
CallEndpoint("AsymmetricKey");

Console.WriteLine();
Console.WriteLine("Press <ENTER> to terminate client.");
Console.ReadLine();

/// <summary>
/// Makes calls to the specified endpoint name in app.config
/// </summary>
/// <param name="endpointName">The endpoint to use from app.config</param>
static void CallEndpoint(string endpointName)  // TODO: check app.config settings
{
    Console.WriteLine("\nCalling endpoint {0}\n", endpointName);

    WSFederationHttpBinding binding = new WSFederationHttpBinding(new WSTrustTokenParameters
    {
        TokenType = Saml2Constants.OasisWssSaml2TokenProfile11,
    });
    var endpointAddress = new EndpointAddress($"https://localhost:5001/CalculatorService/{endpointName}");

    // Create a client with given client endpoint configuration
    CalculatorServiceClient client = new CalculatorServiceClient(binding, endpointAddress);

    // Create new credentials class
    SamlClientCredentials samlCC = new SamlClientCredentials();

    // Set the client certificate. This is the cert that will be used to sign the SAML token in the symmetric proof key case
    samlCC.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindBySubjectName, "Alice");

    // Set the service certificate. This is the cert that will be used to encrypt the proof key in the symmetric proof key case
    samlCC.ServiceCertificate.SetDefaultCertificate(StoreLocation.CurrentUser, StoreName.TrustedPeople, X509FindType.FindBySubjectName, "localhost");

    // Create some claims to put in the SAML assertion
    IList<Claim> claims = new List<Claim>();
    claims.Add(Claim.CreateNameClaim(samlCC.ClientCertificate.Certificate.Subject));
    ClaimSet claimset = new DefaultClaimSet(claims);
    samlCC.Claims = claimset;

    // set new credentials
    client.ChannelFactory.Endpoint.EndpointBehaviors.Remove(typeof(ClientCredentials));
    client.ChannelFactory.Endpoint.EndpointBehaviors.Add(samlCC);

    // Call the Add service operation.
    double value1 = 100.00D;
    double value2 = 15.99D;
    double result = client.Add(value1, value2);
    Console.WriteLine("Add({0},{1}) = {2}", value1, value2, result);

    // Call the Subtract service operation.
    value1 = 145.00D;
    value2 = 76.54D;
    result = client.Subtract(value1, value2);
    Console.WriteLine("Subtract({0},{1}) = {2}", value1, value2, result);

    // Call the Multiply service operation.
    value1 = 9.00D;
    value2 = 81.25D;
    result = client.Multiply(value1, value2);
    Console.WriteLine("Multiply({0},{1}) = {2}", value1, value2, result);

    // Call the Divide service operation.
    value1 = 22.00D;
    value2 = 7.00D;
    result = client.Divide(value1, value2);
    Console.WriteLine("Divide({0},{1}) = {2}", value1, value2, result);

    client.Close();
}
