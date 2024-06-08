using System;
using System.Collections.Generic;

namespace NewmanRingsTwice.DB.NewmanDB
{
    public partial class CcMailAddress
    {
        public int CcMailAddressId { get; set; }
        public string Address { get; set; } = null!;
        public int MailId { get; set; }

        public virtual Mail Mail { get; set; } = null!;
    }
}
