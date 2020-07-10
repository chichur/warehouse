using System;
using System.Collections.Generic;

namespace warehouse.Models
{
    public partial class CargoHistory
    {
        public char Operation { get; set; }
        public DateTime Stamp { get; set; }
        public int IdPlatform { get; set; }
        public int Cargo { get; set; }
    }
}
