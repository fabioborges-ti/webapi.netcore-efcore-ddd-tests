using System.Diagnostics.CodeAnalysis;

namespace Api.CrossCutting.HealthCheck
{
    [ExcludeFromCodeCoverage]
    public class HealthCheck
    {
        public string Status { get; set; }
        public string Component { get; set; }
        public string Description { get; set; }
    }
}
