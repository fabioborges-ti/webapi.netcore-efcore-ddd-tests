using System;
using System.Diagnostics.CodeAnalysis;

namespace Api.Domain.Entities.Base
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Status { get; set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            Status = true;
        }
    }
}
