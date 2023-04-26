// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CoreWcf.Samples.CustomToken
{
    public class CreditCardInfo
    {
        public CreditCardInfo(string cardNumber, string cardIssuer, DateTime expirationDate)
        {
            CardNumber = cardNumber;
            CardIssuer = cardIssuer;
            ExpirationDate = expirationDate;
        }

        public string CardNumber { get; }

        public string CardIssuer { get; }

        public DateTime ExpirationDate { get; }
    }
}
