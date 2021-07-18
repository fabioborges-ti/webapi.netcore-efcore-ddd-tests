using Api.Domain.Entities.Base;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Api.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class UserEntity : BaseEntity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Document { get; set; }
        
        public IEnumerable<AddressEntity> Addresses { get; set; }
        public IEnumerable<PhoneEntity> Phones { get; set; }

        public UserEntity(string name, string email, bool status)
        {
            Name = name;
            Email = email;
            Status = status;
        }

        public void Activate() => Status = true;
        public void Inactivate() => Status = false;
    }
}