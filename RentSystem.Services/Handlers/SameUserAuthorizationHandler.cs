using Microsoft.AspNetCore.Authorization;
using RentSystem.Core.Contracts.Model;
using RentSystem.Core.Enums;

namespace RentSystem.Services.Handlers
{
    public class SameUserAuthorizationHandler : AuthorizationHandler<SameUserRequirement, IUserOwnedResource>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SameUserRequirement requirement, IUserOwnedResource resource)
        {
            if (context.User.IsInRole(Role.Admin.ToString()) || context.User.FindFirst("UserId")?.Value == resource.UserId.ToString())
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }   
    }
    public record SameUserRequirement : IAuthorizationRequirement;
}
