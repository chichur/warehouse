using System;
using System.Collections.Generic;

namespace warehouse
{
    public partial class PlatformsHistory
    {
        public DateTime Stamp { get; set; }
        public int IdStock { get; set; }
        public string NameStock { get; set; }
        public int IdPlatform { get; set; }
        public int Picket { get; set; }
    }
}
