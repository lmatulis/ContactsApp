using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ContactsApp.Client.Models;
using ContactsApp.Data;
using ContactsApp.Helpers;
using System.Security.Claims;

namespace ContactsApp.Components.Account
{
    public class CustomUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> options) : UserClaimsPrincipalFactory<ApplicationUser>(userManager, options)
    {
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            ClaimsIdentity identity = await base.GenerateClaimsAsync(user);

            string profilePicture = user.ImageId.HasValue ? $"/api/uploads/{user.ImageId}" : UploadHelper.DefaultProfilePicture;

            List<Claim> customClaims = [
                new Claim(nameof(UserInfo.ProfilePictureUrl), profilePicture), 
                new Claim(nameof(UserInfo.FirstName), user.FirstName!), 
                new Claim(nameof(UserInfo.LastName), user.LastName!),
                ];

            identity.AddClaims(customClaims);

            return identity;
        }
    }
}
