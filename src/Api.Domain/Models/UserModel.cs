using Api.Domain.Models.Base;
using System.Diagnostics.CodeAnalysis;

namespace Api.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class UserModel : BaseModel
    {
        private string _name;
        private string _email;
        private string _document;
        private bool _status;
        
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Document
        {
            get { return _document; }
            set { _document = value.Replace("-", "").Replace(".", ""); }
        }

        public bool Status
        {
            get { return _status; }
            set { _status = value; }
        }
    }
}
