using System.ComponentModel.DataAnnotations;

namespace TFS_Parser.Entities
{
    public class TFS: ROOT
    {
        [Key]
        public int ID { get; set; }
    }

    public class Alternative: ROOTALTERNATELISTALTITEM
    {
    }
}

