using System;
using System.Diagnostics.CodeAnalysis;

namespace Api.Domain.Dtos.Phone
{
    [ExcludeFromCodeCoverage]
    public class PhoneDtoCreate
    {
        public Guid UserId { get; set; }
        public string Number { get; set; }
    }
}
