using System;
using System.Collections.Generic;

namespace warehouse.Models
{
    public partial class Stocks
    {
        public int IdStock { get; set; }
        public string NameStock { get; set; }
        public int? IdPlatform { get; set; }
        public int? Picket { get; set; }

        public virtual Platforms IdPlatformNavigation { get; set; }
    }
}
