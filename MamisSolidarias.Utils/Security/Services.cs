namespace MamisSolidarias.Utils.Security;

public enum Services
{
    Users,
    Beneficiaries
}

public static class ServiceExtensions
{
    public static string ReadPermission(this Services service) => $"{service}/read";
    public static string WritePermission(this Services service) => $"{service}/write";
}