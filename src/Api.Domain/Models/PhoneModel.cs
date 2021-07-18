using System;
using System.Diagnostics.CodeAnalysis;
using Api.Domain.Models.Base;

namespace Api.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class PhoneModel : BaseModel
    {
        private Guid _userId;
        private string _number;

        public Guid UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }
    }
}
