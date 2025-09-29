using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TFS_Parser.Entities
{
    public class TFS: ROOT
    {
        [Key]
        public int ID_DB { get; set; }
    }
}

