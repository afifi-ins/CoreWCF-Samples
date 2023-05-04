// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
    // Create the custom binding and add an endpoint to the service.
    .AddServiceEndpoint<EchoService, IEchoService>(BindingHelper.CreateMultiFactorAuthenticationBinding(), "EchoService");
    //Binding multipleTokensBinding = BindingHelper.CreateMultiFactorAuthenticationBinding();
    //this.AddServiceEndpoint(typeof(IEchoService), multipleTokensBinding, string.Empty);

    Action<ServiceHostBase> serviceHost = host => ChangeHostBehavior(host);
    builder.ConfigureServiceHostBase<EchoService>(serviceHost);

    // Configure WSDL to be available
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.Run();

void ChangeHostBehavior(ServiceHostBase host)
{
    var srvCredentials = new ServiceCredentials();
    srvCredentials.ServiceCertificate.SetCertificate("CN=localhost");
    //srvCredentials.ServiceCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, "localhost");
    srvCredentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.PeerOrChainTrust;
    host.Description.Behaviors.Add(srvCredentials);
}
