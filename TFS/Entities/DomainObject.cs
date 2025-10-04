using System;
using System.ComponentModel.DataAnnotations;
using Microsoft;
using Pgvector; // NuGet: Npgsql.EntityFrameworkCore.PostgreSQL.Vector

namespace TFS.Entities
{

    public class DomainObject
    {
        [Key]
        public Guid Id { get; set; }
        public string Grp { get; set; } = string.Empty;
        public string? Role { get; set; }
        public string FullText { get; set; } = string.Empty;

        // embedding как float[]
        public float[] Embedding { get; set; } = Array.Empty<float>();
    }
}