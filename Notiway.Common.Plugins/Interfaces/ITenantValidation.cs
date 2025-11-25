using Notiway.Common.Core.Enums;

using System.Security.Claims;

namespace Notiway.Common.Plugins.Interfaces;
public interface ITenantValidation
{
    Task<Processing> ValidateAsync(string tenantId, string userId, IEnumerable<Claim> claims);
}
