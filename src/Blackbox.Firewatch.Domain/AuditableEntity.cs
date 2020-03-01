using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Domain
{
    public abstract class AuditableEntity
    {
        public string CreatedById { get; set; }
        public Person CreatedBy { get; set; }

        public DateTime Created { get; set; }

        public string LastModifiedById { get; set; }
        public Person LastModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
