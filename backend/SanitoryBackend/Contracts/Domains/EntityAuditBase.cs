using System;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Enums;
using Contracts.Interfaces;

namespace Contracts.Domains
{
    public class EntityAuditBase<Tkey> : EntityBase<Tkey>, IAuditable, IUserTracking
    {
        public DateTimeOffset? CreatedDate { get; set; }

        public DateTimeOffset? LastModifiedDate { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? CreatedBy { get; set; }

         [Column(TypeName = "nvarchar(50)")]
        public string? LastModifiedBy { get; set; }

        public Status Status { get; set; }

        public int Order { get; set; }
    }
}
