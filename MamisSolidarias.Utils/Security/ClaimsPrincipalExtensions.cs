using System.Security.Claims;
using FastEndpoints.Security;

namespace MamisSolidarias.Utils.Security;

/// <summary>
/// A set of extension methods for the ClaimsPrincipal class
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// It determines if the user is an admin or is the owner of the account
    /// </summary>
    /// <param name="user">Claims principal</param>
    /// <param name="permissionLevel">Required permission level</param>
    /// <param name="accountOwnerId">Id of the owner of the account</param>
    /// <returns></returns>
    public static bool HasPermissionOrIsAccountOwner(this ClaimsPrincipal user, string permissionLevel, int accountOwnerId)
    {
        var isUserAdmin = user.HasPermission(permissionLevel);
        var isUserOwnerOfAccount = user.GetUserId() == accountOwnerId;

        return isUserAdmin || isUserOwnerOfAccount;
    }

    /// <summary>
    /// It extracts the user ID from the users' claims
    /// </summary>
    /// <param name="user">Claims principal</param>
    /// <returns>The ID, if it was registered</returns>
    public static int? GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.Claims.FirstOrDefault(t => t.Type is "Id");
        
        if (claim is null)
            return null;
        
        if (int.TryParse(claim.Value, out var id))
            return id;
        
        return null;
    }
}