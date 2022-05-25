using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TFS_Parser.Entities
{
    [XmlInclude(typeof(ROOT))]
    [Serializable]
    public class TFS: ROOT
    {
        [Key]
        public int ID { get; set; }
    }
}

