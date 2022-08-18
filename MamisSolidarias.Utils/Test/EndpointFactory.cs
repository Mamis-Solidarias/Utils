using System.Security.Claims;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace MamisSolidarias.Utils.Test;

public static class EndpointFactory
{
    /// <summary>
    /// It creates an endpoint builder to mock FastEndpoints
    /// </summary>
    /// <param name="constructorParameters">Constructor parameters in the same order as the definition</param>
    /// <typeparam name="TEndpoint">Type of the endpoint to mock</typeparam>
    /// <returns>And endpoint builder</returns>
    public static EndpointConfiguration<TEndpoint> CreateEndpoint<TEndpoint>(params object?[] constructorParameters) 
        where TEndpoint : class, IEndpoint
    {
        return new EndpointConfiguration<TEndpoint>(constructorParameters);
    }
}

public class EndpointConfiguration<TEndpoint> where TEndpoint : class, IEndpoint
{
    /// <summary>
    /// It initializes the endpoint builder
    /// </summary>
    /// <param name="constructorParameters">Constructor parameters in the same order as the definition</param>
    public EndpointConfiguration(object?[] constructorParameters)
    {
        ConstructorParameters = constructorParameters;
    }
    private object?[] ConstructorParameters { get; }
    private ClaimsPrincipal? UserClaims { get; set; }
    private Action<ServiceCollection>? DependencyInjector { get; set; }
    
    /// <summary>
    /// It adds services to the endpoint via dependency injection
    /// </summary>
    /// <param name="services">Action to inject dependencies</param>
    public EndpointConfiguration<TEndpoint> WithInjectedServices(Action<ServiceCollection> services)
    {
        DependencyInjector = services;
        return this;
    }

    /// <summary>
    /// It adds Claims to the endpoint
    /// </summary>
    /// <param name="claims">A set of claims to be used within the endpoint</param>
    public EndpointConfiguration<TEndpoint> WithClaims(ClaimsPrincipal claims)
    {
        UserClaims = claims;
        return this;
    }
    
    /// <summary>
    /// It generates a mocked endpoint
    /// </summary>
    /// <returns>A mocked FastEndpoint endpoint</returns>
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
                    services.AddSingleton(new Mock<ILogger<TEndpoint>>().Object);
                    ctx.RequestServices = services.BuildServiceProvider();
                }
            }, 
            ConstructorParameters
        );
}
