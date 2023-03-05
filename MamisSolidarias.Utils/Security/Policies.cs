using Microsoft.AspNetCore.Authorization;

namespace MamisSolidarias.Utils.Security;


/// <summary>
/// Security policies to control access to the services
/// </summary>
public static class Policies
{
    /// <summary>
    /// This policy allows access to users with read capabilities to the system
    /// </summary>
    public static string CanRead => "CanRead";

    /// <summary>
    /// This policy allows access to users with write capabilities to the system
    /// </summary>
    public static string CanWrite => "CanWrite";

    /// <summary>
    /// This policy allows access to users with both read and write capabilities to the system
    /// </summary>
    public static string All => "All";
}

/// <summary>
/// A set of PolicyBuilder extensions
/// </summary>
public static class PolicyExtensions
{
    /// <summary>
    /// Claim type for permissions
    /// </summary>
    public const string PermissionClaimType = "permission";
    
    /// <summary>
    /// It sets up policies for a given service
    /// </summary>
    /// <param name="options">Authorization builder</param>
    /// <param name="service">Service to set up</param>
    /// <returns></returns>
    public static void ConfigurePolicies(this AuthorizationOptions options, Services service)
    {
        options.AddPolicy(Policies.CanRead,
            t=> t.RequireClaim(PermissionClaimType, service.ReadPermission()));
        
        options.AddPolicy(Policies.CanWrite,
            t=> t.RequireClaim(PermissionClaimType, service.WritePermission()));
        
        options.AddPolicy(Policies.All,
            t=> 
                t.RequireClaim(PermissionClaimType, service.ReadPermission())
                .RequireClaim(PermissionClaimType, service.WritePermission()));
        
    }
}