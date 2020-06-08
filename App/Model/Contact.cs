using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace crmme.Model
{
    public class Contact
    {
        public string  id { get; set; }
        public string givenName { get; set; }
        public string surname { get; set; }
        public string ParentFolderId { get; set; }
        public List<EmailAddress> EmailAddresses { get; set; }
    }
}
