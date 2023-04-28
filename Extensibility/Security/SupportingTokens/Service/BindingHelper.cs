﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CoreWCF.Security.Tokens;

namespace CoreWcf.Samples.SupportingTokens
{
    public static class BindingHelper
    {
        public static Binding CreateMultiFactorAuthenticationBinding()
        {
            HttpTransportBindingElement httpTransport = new HttpsTransportBindingElement();

            // the message security binding element will be configured to require 2 tokens:
            // 1) A username-password encrypted with the service token
            // 2) A client certificate used to sign the message

            // Instantiate a binding element that will require the username/password token in the message (encrypted with the server cert)
            TransportSecurityBindingElement messageSecurity = SecurityBindingElement.CreateUserNameOverTransportBindingElement();

            // Create supporting token parameters for the client X509 certificate.
            X509SecurityTokenParameters clientX509SupportingTokenParameters = new X509SecurityTokenParameters();
            // Specify that the supporting token is passed in message send by the client to the service
            clientX509SupportingTokenParameters.InclusionMode = SecurityTokenInclusionMode.AlwaysToRecipient;
            // Turn off derived keys
            clientX509SupportingTokenParameters.RequireDerivedKeys = false;
            // Augment the binding element to require the client's X509 certificate as an endorsing token in the message
            messageSecurity.EndpointSupportingTokenParameters.Endorsing.Add(clientX509SupportingTokenParameters);

            // Create a CustomBinding based on the constructed security binding element.
            return new CustomBinding(messageSecurity, httpTransport);
        }
    }
}

