using System;

namespace MyTinyBank.Core.Model
{
    public class AuditInfo
    {
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }

        public AuditInfo()
        {
            Created = DateTimeOffset.Now;
        }
    }
}
