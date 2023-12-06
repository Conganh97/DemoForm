using Common.Enums;
using Contracts.Interfaces;

namespace Contracts.Domains
{
    public class DtoBase : IDateTracking
    {
        public DateTimeOffset? CreatedDate { get; set; }

        public DateTimeOffset? LastModifiedDate { get; set; }

        public Status Status { get; set; }
        public int? Order { get; set; }
    }
}
