// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CoreWcf.Samples.CustomToken
{
    public class Constants
    {
        public const string TestCreditCardIssuer = "TestCreditCardIssuer";

        public const string CreditCardNumberClaim = "http://samples.microsoft.com/wcf/security/Extensibility/Claims/CreditCardNumber";
        public const string CreditCardTokenType = "http://samples.microsoft.com/wcf/security/Extensibility/Tokens/CreditCardToken";

        internal const string CreditCardTokenPrefix = "cct";
        internal const string CreditCardTokenNamespace = "http://samples.microsoft.com/wcf/security/Extensibility/";
        internal const string CreditCardTokenName = "CreditCardToken";
        internal const string Id = "Id";
        internal const string WsUtilityPrefix = "wsu";
        internal const string WsUtilityNamespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
        internal const string CreditCardNumberElementName = "CreditCardNumber";
        internal const string CreditCardIssuerElementName = "CreditCardIssuer";
        internal const string CreditCardExpirationElementName = "Expires";

    }
}
