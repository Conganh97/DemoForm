using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contracts.Interfaces
{
    public interface IUserTracking
    {
        [Column(TypeName = "nvarchar(50)")]
        public string? CreatedBy { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? LastModifiedBy { get; set; }
    }
}
