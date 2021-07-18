using System;
using System.Diagnostics.CodeAnalysis;

namespace Api.Domain.Models.Base
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseModel
    {
        private Guid _id;
        private DateTime _createdAt;
        private DateTime _updatedAt;

        public Guid Id
        {
            get => _id;
            set => _id = value;
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = DateTime.UtcNow;
        }

        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set => _updatedAt = value;
        }
    }
}
