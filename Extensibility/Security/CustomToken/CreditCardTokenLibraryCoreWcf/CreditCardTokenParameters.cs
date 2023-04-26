// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CoreWCF.IdentityModel;
using CoreWCF.Security.Tokens;

namespace CoreWcf.Samples.CustomToken
{
    /// <summary>
    /// CreditCardTokenParameters for use with the Credit Card Token
    /// </summary>
    public class CreditCardTokenParameters : SecurityTokenParameters
    {
        string _issuer;

        public CreditCardTokenParameters() : this((string)null) { }

        public CreditCardTokenParameters(string issuer) : base()
        {
            _issuer = issuer;
        }

        protected CreditCardTokenParameters(CreditCardTokenParameters other)
            : base(other)
        {
            _issuer = other._issuer;
        }

        protected override SecurityTokenParameters CloneCore()
        {
            return new CreditCardTokenParameters(this);
        }

        public string Issuer
        {
            get { return _issuer; }
        }

        protected override void InitializeSecurityTokenRequirement(SecurityTokenRequirement requirement)
        {
            requirement.TokenType = Constants.CreditCardTokenType;
            return;
        }

        // A credit card token has no crypto, no windows identity and supports only client authentication
        protected override bool HasAsymmetricKey { get { return false; } }
        protected override bool SupportsClientAuthentication { get { return true; } }
        protected override bool SupportsClientWindowsIdentity { get { return false; } }
        protected override bool SupportsServerAuthentication { get { return false; } }

        protected override SecurityKeyIdentifierClause CreateKeyIdentifierClause(SecurityToken token, SecurityTokenReferenceStyle referenceStyle)
        {
            if (referenceStyle == SecurityTokenReferenceStyle.Internal)
            {
                return token.CreateKeyIdentifierClause<LocalIdKeyIdentifierClause>();
            }
            else
            {
                throw new NotSupportedException("External references are not supported for credit card tokens");
            }
        }
    }
}

