using System;
using System.Collections.Generic;

namespace warehouse.Models
{
    public partial class Platforms
    {
        public Platforms()
        {
            Stocks = new HashSet<Stocks>();
        }

        public int IdPlatform { get; set; }
        public int? Cargo { get; set; }

        public virtual ICollection<Stocks> Stocks { get; set; }
    }
}
