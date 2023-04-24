// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography.X509Certificates;
using CoreWCF.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder();

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
        serviceOptions.BaseAddresses.Add(new Uri("https://localhost/CalculatorService"));
    })
    // Add WSHttpBinding endpoint
    .AddServiceEndpoint<CalculatorService, ICalculatorService>(ServiceWSFederationHttpBinding(SecurityKeyType.SymmetricKey), "SymmetricKey")
    .AddServiceEndpoint<CalculatorService, ICalculatorService>(ServiceWSFederationHttpBinding(SecurityKeyType.AsymmetricKey), "AsymmetricKey");

    Action<ServiceHostBase> serviceHost = host => ChangeHostBehavior(host);
    builder.ConfigureServiceHostBase<CalculatorService>(serviceHost);

    // Configure WSDL to be available
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.Run();

static Binding ServiceWSFederationHttpBinding(SecurityKeyType securityKeyType)
{
    WSFederationHttpBinding binding = new WSFederationHttpBinding(WSFederationHttpSecurityMode.TransportWithMessageCredential);
    binding.Security.Message.NegotiateServiceCredential = false;
    binding.Security.Message.IssuedKeyType = securityKeyType;
    binding.Security.Message.IssuedTokenType = "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV1.1";
    return binding;
}

void ChangeHostBehavior(ServiceHostBase host)
{
    var srvCredentials = new ServiceCredentials();
    srvCredentials.ServiceCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, "localhost");
    // Set validation mode for issuer certificates to allow certificates in the trusted people store
    host.Credentials.IssuedTokenAuthentication.CertificateValidationMode = X509CertificateValidationMode.PeerTrust;
    // Set allowUntrustedRsaIssuers to true to allow self - signed, asymmetric key based SAML tokens 
    host.Credentials.IssuedTokenAuthentication.AllowUntrustedRsaIssuers = true;
    host.Description.Behaviors.Add(srvCredentials);

    // Add "Alice" to the list of certs trusted to issue SAML tokens 
    X509Store store = new X509Store(StoreName.TrustedPeople, StoreLocation.LocalMachine);
    X509Certificate2Collection certificates = null;
    try
    {
        store.Open(OpenFlags.ReadOnly);
        certificates = store.Certificates.Find(X509FindType.FindBySubjectName, "Alice", false);
        host.Credentials.IssuedTokenAuthentication.KnownCertificates.Add(certificates.FirstOrDefault());
    }
    finally
    {
        store.Close();
    }
}
