using System.ComponentModel.DataAnnotations;

namespace TFS_Parser.Entities
{
    //used by postgres
    public class TFS: ROOT
    {
        [Key]
        public int ID { get; set; }
    }
}

