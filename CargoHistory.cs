using System;
using System.Collections.Generic;

namespace warehouse
{
    public partial class CargoHistory
    {
        public DateTime Stamp { get; set; }
        public int IdPlatform { get; set; }
        public int Cargo { get; set; }
    }
}
