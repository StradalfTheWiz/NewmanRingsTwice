using System;
using System.Collections.Generic;

namespace NewmanRingsTwice.DB.NewmanDB
{
    public partial class ImportanceType
    {
        public ImportanceType()
        {
            Mail = new HashSet<Mail>();
        }

        public int ImportanceTypeId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Mail> Mail { get; set; }
    }
}
