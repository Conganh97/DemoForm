using Common.Shared.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Identity.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ClaimRequirementAttribute : TypeFilterAttribute
{
    public ClaimRequirementAttribute(RoleCode[] role)
        : base(typeof(ClaimRequirementFilter))
    {
        Arguments = new object[] { role };
    }
}