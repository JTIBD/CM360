using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FMAplication.Core;

namespace FMAplication.Domain
{
    public class Reason: AuditableEntity<int>
    {
        protected Reason()
        {
        }

        public string Name { get; set; }
        public string ReasonInEnglish { get; set; }
        public string ReasonInBangla { get; set; }
        public List<ReasonReasonTypeMapping> ReasonReasonTypeMappings { get; set; }
    }
}
