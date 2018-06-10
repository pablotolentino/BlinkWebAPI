using System;
using System.Collections.Generic;

namespace Blink.Core.Entities
{
    public partial class Affiliate
    {
        public long PersonId { get; set; }
        public string CodeAffiliate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public Person Person { get; set; }
    }
}
