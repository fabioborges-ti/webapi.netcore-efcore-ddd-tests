using System;
using System.Diagnostics.CodeAnalysis;

namespace Api.Domain.Dtos.Address
{
    [ExcludeFromCodeCoverage]
    public class AddressDtoCreate
    {
        public Guid UserId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}
