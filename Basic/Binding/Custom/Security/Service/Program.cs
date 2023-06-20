// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography.X509Certificates;

const int HttpPort = 5000;
const int NetTcpPort = 8000;

var builder = WebApplication.CreateBuilder();

builder.WebHost
.UseKestrel(options =>
{
    options.ListenAnyIP(HttpPort);
})
.UseNetTcp(NetTcpPort);

//Enable CoreWCF Services, with metadata (WSDL) support
builder.Services.AddServiceModelServices()
    .AddServiceModelMetadata()
    .AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();

app.UseServiceModel(builder =>
{
    // Add the Calculator Service
    builder.AddService<CalculatorService>(serviceOptions =>
       {
           serviceOptions.BaseAddresses.Clear();
           // Set the default host name:port in generated WSDL and the base path for the address 
           serviceOptions.BaseAddresses.Add(new Uri($"http://localhost:{HttpPort}/CalculatorService"));
           serviceOptions.BaseAddresses.Add(new Uri($"https://localhost:{5001}/CalculatorService"));
           serviceOptions.BaseAddresses.Add(new Uri($"net.tcp://localhost:{NetTcpPort}/CalculatorService"));
       });

    BasicHttpBinding basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
    HttpTransportBindingElement httpTransportBindingElement = basicHttpBinding.CreateBindingElements().Find<HttpTransportBindingElement>();

    MessageEncodingBindingElement encodingBindingElement = new TextMessageEncodingBindingElement(MessageVersion.Soap12WSAddressing10, System.Text.Encoding.UTF8);
    httpTransportBindingElement.TransferMode = TransferMode.Streamed;
    CustomBinding binding = new CustomBinding(new BindingElement[]
    {
                encodingBindingElement,
                httpTransportBindingElement
    });
    builder.AddServiceEndpoint<CalculatorService, ICalculatorService>(binding, "customBinding");

    Action<ServiceHostBase> serviceHost = host => ChangeHostBehavior(host);
    builder.ConfigureServiceHostBase<CalculatorService>(serviceHost);

    // Configure WSDL to be available
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.Run();

void ChangeHostBehavior(ServiceHostBase host)
{
    var srvCredentials = new ServiceCredentials();
    srvCredentials.ServiceCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, "localhost");
    host.Description.Behaviors.Add(srvCredentials);
}
