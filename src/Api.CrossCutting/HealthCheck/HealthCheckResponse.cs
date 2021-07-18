using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Api.CrossCutting.HealthCheck
{
    [ExcludeFromCodeCoverage]
    public class HealthCheckResponse
    {
        public string Status { get; set; }
        public IEnumerable<HealthCheck> Checks { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
