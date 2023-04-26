// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;

namespace CoreWcf.Samples.CustomToken
{
    class CreditCardToken : SecurityToken
    {
        DateTime _effectiveTime = DateTime.UtcNow;
        string _id;
        ReadOnlyCollection<SecurityKey> _securityKeys;

        public CreditCardToken(CreditCardInfo cardInfo) : this(cardInfo, Guid.NewGuid().ToString()) { }

        public CreditCardToken(CreditCardInfo cardInfo, string id)
        {
            CardInfo = cardInfo ?? throw new ArgumentNullException(nameof(cardInfo));
            _id = id ?? throw new ArgumentNullException(nameof(id));

            // the credit card token is not capable of any crypto
            _securityKeys = new ReadOnlyCollection<SecurityKey>(new List<SecurityKey>());
        }

        public CreditCardInfo CardInfo { get; }

        public override ReadOnlyCollection<SecurityKey> SecurityKeys { get { return _securityKeys; } }

        public override DateTime ValidFrom { get { return _effectiveTime; } }
        public override DateTime ValidTo { get { return CardInfo.ExpirationDate; } }
        public override string Id { get { return _id; } }
    }
}
