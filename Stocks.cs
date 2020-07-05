using System.ComponentModel.DataAnnotations.Schema;

namespace warehouse
{
    public partial class Stocks
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdStock { get; set; }
        public string NameStock { get; set; }
        public int? IdPlatform { get; set; }
        public int? Picket { get; set; }
        public virtual Platforms IdPlatformNavigation { get; set; }
    }
}
