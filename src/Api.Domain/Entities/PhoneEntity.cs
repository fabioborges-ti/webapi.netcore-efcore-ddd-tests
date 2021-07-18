using Api.Domain.Entities.Base;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Api.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class PhoneEntity : BaseEntity
    {
        public string Number { get; set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
    }
}
