using System.Security.Claims;

namespace ContactsApp.Components.Account
{
    public static class ClaimsPrincipalExtensions
    {

        public static string? GetUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

    }
}
