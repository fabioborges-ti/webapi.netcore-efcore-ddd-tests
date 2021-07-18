using System;
using System.Diagnostics.CodeAnalysis;
using Api.Domain.Models.Base;

namespace Api.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class AddressModel : BaseModel
    {
        private Guid _userId;
        private string _address;
        private string _city { get; set; }
        private string _state { get; set; }
        private string _zip { get; set; }

        public Guid UserId
        {
            get => _userId;
            set => _userId = value;
        }

        public string Address
        {
            get => _address;
            set => _address = value;
        }

        public string City
        {
            get => _city;
            set => _city = value;
        }

        public string State
        {
            get => _state;
            set => _state = value;
        }

        public string Zip
        {
            get => _zip;
            set => _zip = string.IsNullOrEmpty(value) ? "--" : value;
        }
    }
}
