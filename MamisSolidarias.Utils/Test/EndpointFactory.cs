using System.Security.Claims;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;

namespace MamisSolidarias.Utils.Test;

public static class EndpointFactory
{
    
    public static EndpointConfiguration<TEndpoint> CreateEndpoint<TEndpoint>(params object?[] constructorParameters) 
        where TEndpoint : class, IEndpoint
    {
        return new EndpointConfiguration<TEndpoint>(constructorParameters);
    }
}

public class EndpointConfiguration<TEndpoint> where TEndpoint : class, IEndpoint
{
    public EndpointConfiguration(object?[] constructorParameters)
    {
        ConstructorParameters = constructorParameters;
    }
    private object?[] ConstructorParameters { get; }
    private ClaimsPrincipal? UserClaims { get; set; }
    private Action<ServiceCollection>? DependencyInjector { get; set; }
    
    public EndpointConfiguration<TEndpoint> WithInjectedServices(Action<ServiceCollection> services)
    {
        DependencyInjector = services;
        return this;
    }

    public EndpointConfiguration<TEndpoint> WithClaims(ClaimsPrincipal claims)
    {
        UserClaims = claims;
        return this;
    }
    
    public TEndpoint Build() =>
        Factory.Create<TEndpoint>(
            ctx =>
            {
                if (UserClaims is not null)
                    ctx.User = UserClaims;

                if (DependencyInjector is not null)
                {
                    var services = new ServiceCollection();
                    DependencyInjector.Invoke(services);
                    ctx.RequestServices = services.BuildServiceProvider();
                }
            }, 
            ConstructorParameters
        );
}
