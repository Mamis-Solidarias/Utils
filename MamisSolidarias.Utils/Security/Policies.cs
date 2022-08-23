using FastEndpoints.Security;
using Microsoft.AspNetCore.Authorization;

namespace MamisSolidarias.Utils.Security;


/// <summary>
/// Security policies to control access to the services
/// </summary>
public enum Policies
{
    /// <summary>
    /// This policy allows access to users with read capabilities to the system
    /// </summary>
    CanRead,
    
    /// <summary>
    /// This policy allows access to users with write capabilities to the system
    /// </summary>
    CanWrite,
    
    /// <summary>
    /// This policy allows access to users with both read and write capabilities to the system
    /// </summary>
    All
}

/// <summary>
/// A set of PolicyBuilder extensions
/// </summary>
public static class PolicyExtensions
{
    /// <summary>
    /// It sets up policies for a given service
    /// </summary>
    /// <param name="options">Authorization builder</param>
    /// <param name="service">Service to set up</param>
    /// <returns></returns>
    public static void ConfigurePolicies(this AuthorizationOptions options, Services service)
    {
        
        options.AddPolicy(Policies.CanRead.ToString(),
            t=> t.RequireClaim(Constants.PermissionsClaimType, service.ReadPermission()));
        
        options.AddPolicy(Policies.CanWrite.ToString(),
            t=> t.RequireClaim(Constants.PermissionsClaimType, service.WritePermission()));
        
        options.AddPolicy(Policies.All.ToString(),
            t=> 
                t.RequireClaim(Constants.PermissionsClaimType, service.ReadPermission())
                .RequireClaim(Constants.PermissionsClaimType, service.WritePermission()));
        
    }
}