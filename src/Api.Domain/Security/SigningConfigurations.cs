﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Api.Domain.Security
{
    [ExcludeFromCodeCoverage]
    public class SigningConfigurations
    {
        public SecurityKey Key { get; set; }
        public SigningCredentials SigningCredentials { get; set; }

        public SigningConfigurations()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                Key = new RsaSecurityKey(provider.ExportParameters(true));
            }

            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}
