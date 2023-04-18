// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography.X509Certificates;
using CoreWCF.IdentityModel.Claims;

namespace CoreWcf.Samples.SupportingTokens
{
    // Service class which implements the service contract interface.
    // Added code to write output to the console window
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class EchoService : IEchoService
    {
        public string Echo()
        {
            string userName;
            string certificateSubjectName;
            GetCallerIdentities(OperationContext.Current.ServiceSecurityContext, out userName, out certificateSubjectName);
            return string.Format("Hello {0}, {1}", userName, certificateSubjectName);
        }

        public void Dispose()
        {
        }

        bool TryGetClaimValue<TClaimResource>(ClaimSet claimSet, string claimType, out TClaimResource resourceValue)
            where TClaimResource : class
        {
            resourceValue = default(TClaimResource);
            IEnumerable<Claim> matchingClaims = claimSet.FindClaims(claimType, Rights.PossessProperty);
            if (matchingClaims == null)
                return false;
            IEnumerator<Claim> enumerator = matchingClaims.GetEnumerator();
            if (enumerator.MoveNext())
            {
                resourceValue = (enumerator.Current.Resource == null) ? null : (enumerator.Current.Resource as TClaimResource);
                return true;
            }
            else
            {
                return false;
            }
        }

        // Returns the username and certificate subject name provided by the client
        void GetCallerIdentities(ServiceSecurityContext callerSecurityContext, out string userName, out string certificateSubjectName)
        {
            userName = null;
            certificateSubjectName = null;

            // Look in all the claimsets in the authorization context
            foreach (ClaimSet claimSet in callerSecurityContext.AuthorizationContext.ClaimSets)
            {
                if (claimSet is WindowsClaimSet)
                {
                    // Try to find a Name claim. This will have been generated from the windows username.
                    string tmpName;
                    if (TryGetClaimValue<string>(claimSet, ClaimTypes.Name, out tmpName))
                    {
                        userName = tmpName;
                    }
                }
                else if (claimSet is X509CertificateClaimSet)
                {
                    // Try to find an X500DisinguishedName claim. This will have been generated from the client certificate.
                    X500DistinguishedName tmpDistinguishedName;
                    if (TryGetClaimValue(claimSet, ClaimTypes.X500DistinguishedName, out tmpDistinguishedName))
                    {
                        certificateSubjectName = tmpDistinguishedName.Name;
                    }
                }
            }
        }
    }
}
