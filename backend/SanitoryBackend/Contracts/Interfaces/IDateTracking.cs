using System;

namespace Contracts.Interfaces
{
    public interface IDateTracking
    {
        DateTimeOffset? CreatedDate { get; set; }

        DateTimeOffset? LastModifiedDate { get; set; }
    }
}
