// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CoreWCF.IdentityModel.Claims;
using CoreWCF.IdentityModel.Policy;

namespace CoreWcf.Samples.CustomToken
{
    public class CreditCardTokenAuthorizationPolicy : IAuthorizationPolicy
    {
        string _id;
        ClaimSet _issuer;
        IEnumerable<ClaimSet> _issuedClaimSets;

        public CreditCardTokenAuthorizationPolicy(ClaimSet issuedClaims)
        {
            if (issuedClaims == null)
                throw new ArgumentNullException("issuedClaims");
            _issuer = issuedClaims.Issuer;
            _issuedClaimSets = new ClaimSet[] { issuedClaims };
            _id = Guid.NewGuid().ToString();
        }

        public ClaimSet Issuer { get { return _issuer; } }

        public string Id { get { return _id; } }

        public bool Evaluate(EvaluationContext context, ref object state)
        {
            foreach (ClaimSet issuance in _issuedClaimSets)
            {
                context.AddClaimSet(this, issuance);
            }

            return true;
        }
    }
}

