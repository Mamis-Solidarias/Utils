namespace MamisSolidarias.Utils.Security;

/// <summary>
/// A list of all the services on the network
/// </summary>
public enum Services
{
    /// <summary>
    /// Users service
    /// </summary>
    Users,
    /// <summary>
    /// Beneficiaries service
    /// </summary>
    Beneficiaries,
    /// <summary>
    /// Donors service
    /// </summary>
    Donors
}

/// <summary>
/// A list of extension methods for the services
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// It calculates the read permission string
    /// </summary>
    /// <param name="service">Service to create the read permission string for</param>
    /// <returns>The read permission string</returns>
    public static string ReadPermission(this Services service) => $"{service}/read";
    
    /// <summary>
    /// It calculates the write permission string
    /// </summary>
    /// <param name="service">Service to create the write permission string for</param>
    /// <returns>The write permission string</returns>
    public static string WritePermission(this Services service) => $"{service}/write";
}