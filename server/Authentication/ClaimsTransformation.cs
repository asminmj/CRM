using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication;

namespace B
{
    public class ClaimsTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = principal.Identity as WindowsIdentity;

            foreach (var groupId in identity.Groups)
            {
                try
                {
                    var group = groupId.Translate(typeof(NTAccount));

                    identity.AddClaim(new Claim(identity.RoleClaimType, group.Value.Split("\\").Last()));
                    identity.AddClaim(new Claim(identity.RoleClaimType, group.Value));
                }
                catch (IdentityNotMappedException ex)
                {
                }
            }

            return Task.FromResult(principal);
        }
    }
}
