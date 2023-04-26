// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder();

//Enable CoreWCF Services, with metadata (WSDL) support
builder.Services.AddServiceModelServices()
    .AddServiceModelMetadata()
    .AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();

app.UseServiceModel(builder =>
{
    // Add the Calculator Service
    builder.AddService<EchoService>(serviceOptions => { })
    // Register a credit card binding for the endpoint.
    .AddServiceEndpoint<EchoService, IEchoService>(BindingHelper.CreateCreditCardBinding(), "EchoService");

    Action<ServiceHostBase> serviceHost = host => ChangeHostBehavior(host);
    builder.ConfigureServiceHostBase<EchoService>(serviceHost);

    // Configure WSDL to be available
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.Run();

void ChangeHostBehavior(ServiceHostBase host)
{
    string creditCardFile = Path.Combine(Directory.GetCurrentDirectory(), "CreditCardFile.txt");
    if (!File.Exists(creditCardFile))
    {
        throw new FileNotFoundException("creditCardFile file does not exist.");
    }

    CreditCardServiceCredentials serviceCredentials = new CreditCardServiceCredentials(creditCardFile);
    serviceCredentials.ServiceCertificate.SetCertificate("CN=localhost", StoreLocation.LocalMachine, StoreName.My);
    host.Description.Behaviors.Remove((typeof(ServiceCredentials)));
    host.Description.Behaviors.Add(serviceCredentials);
}
