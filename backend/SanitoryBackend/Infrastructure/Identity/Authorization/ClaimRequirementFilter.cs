using System.Text.Json;
using Common.Shared.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Identity.Authorization;

public class ClaimRequirementFilter : IAuthorizationFilter
{
    private RoleCode[] _roles;

    public ClaimRequirementFilter(params RoleCode[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var permissionsClaim = context.HttpContext.User.Claims
            .SingleOrDefault(c => c.Type.Equals(SystemConstants.Claims.Permissions));
        if (permissionsClaim != null)
        {
            var permissions = JsonSerializer.Deserialize<List<string>>(permissionsClaim.Value);
            // if (!permissions.Contains(_roleCode.ToString()))
            //     context.Result = new ForbidResult();
        }
        else
        {
            context.Result = new ForbidResult();
        }
    }
}