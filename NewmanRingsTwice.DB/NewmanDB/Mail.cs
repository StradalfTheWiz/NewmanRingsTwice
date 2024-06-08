using System;
using System.Collections.Generic;

namespace NewmanRingsTwice.DB.NewmanDB
{
    public partial class Mail
    {
        public Mail()
        {
            CcMailAddresses = new HashSet<CcMailAddress>();
        }

        public int MailId { get; set; }
        public string FromMail { get; set; } = null!;
        public string ToMail { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public DateTime DatetimeCreated { get; set; }
        public int ImportanceTypeId { get; set; }
        public string MailContent { get; set; } = null!;

        public virtual ImportanceType ImportanceType { get; set; } = null!;
        public virtual ICollection<CcMailAddress> CcMailAddresses { get; set; }
    }
}
