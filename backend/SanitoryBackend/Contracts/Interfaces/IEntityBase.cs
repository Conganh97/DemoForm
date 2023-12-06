using System;
using System.ComponentModel.DataAnnotations;

namespace Contracts.Interfaces
{
    public interface IEntityBase<T>
    {
        [Key]
        T Id { get; set; }
    }
}

