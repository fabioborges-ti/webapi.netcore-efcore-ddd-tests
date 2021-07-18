using Api.Domain.Entities.Base;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Api.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class AddressEntity : BaseEntity
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
    }
}
