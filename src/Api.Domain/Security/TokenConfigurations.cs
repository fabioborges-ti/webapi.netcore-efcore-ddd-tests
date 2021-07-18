using System.Diagnostics.CodeAnalysis;

namespace Api.Domain.Security
{
    [ExcludeFromCodeCoverage]
    public class TokenConfigurations
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int Seconds { get; set; }
    }
}
