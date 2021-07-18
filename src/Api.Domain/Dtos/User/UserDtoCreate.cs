using System.Diagnostics.CodeAnalysis;

namespace Api.Domain.Dtos.User
{
    [ExcludeFromCodeCoverage]
    public class UserDtoCreate
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
    }
}
