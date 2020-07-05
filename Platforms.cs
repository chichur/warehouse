using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace warehouse
{
    public partial class Platforms
    {
        public Platforms()
        {
            Stocks = new HashSet<Stocks>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPlatform { get; set; }
        public int? Cargo { get; set; }


        public virtual ICollection<Stocks> Stocks { get; set; }
    }
}
