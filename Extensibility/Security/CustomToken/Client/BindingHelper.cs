// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ServiceModel.Channels;

namespace CoreWcf.Samples.CustomToken
{
    public static class BindingHelper
    {
        public static Binding CreateCreditCardBinding()
        {
            HttpTransportBindingElement httpTransport = new HttpTransportBindingElement();

            // the message security binding element will be configured to require a credit card
            // token that is encrypted with the service's certificate 
            SymmetricSecurityBindingElement messageSecurity = new SymmetricSecurityBindingElement();
            messageSecurity.EndpointSupportingTokenParameters.SignedEncrypted.Add(new CreditCardTokenParameters());
            X509SecurityTokenParameters x509ProtectionParameters = new X509SecurityTokenParameters();
            x509ProtectionParameters.InclusionMode = SecurityTokenInclusionMode.Never;
            messageSecurity.ProtectionTokenParameters = x509ProtectionParameters;
            return new CustomBinding(messageSecurity, httpTransport);
        }
    }
}

