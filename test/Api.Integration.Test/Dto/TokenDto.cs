using System;

namespace Api.Integration.Test.Dto
{
    public class TokenDto
    {
        public bool Authenticated { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expiration { get; set; }
        public string AccessToken { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
    }
}
