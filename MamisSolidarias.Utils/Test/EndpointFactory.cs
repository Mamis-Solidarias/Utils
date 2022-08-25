using System.Security.Claims;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace MamisSolidarias.Utils.Test;

/// <summary>
/// An Endpoint Factory
/// </summary>
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

/// <summary>
/// Endpoint Factory builder
/// </summary>
/// <typeparam name="TEndpoint">Type of the endpoint to build</typeparam>
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
    private List<Action<ServiceCollection>> DependencyInjector { get; set; } = new();
    
    /// <summary>
    /// It adds services to the endpoint via dependency injection
    /// </summary>
    /// <param name="services">Action to inject dependencies</param>
    public EndpointConfiguration<TEndpoint> WithInjectedServices(Action<ServiceCollection> services)
    {
        DependencyInjector.Add(services);
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
    /// It registers a logger using dependency injection
    /// </summary>
    /// <typeparam name="TRequest">Type of the request</typeparam>
    /// <typeparam name="TResponse">Type of the response</typeparam>
    /// <returns></returns>
    public EndpointConfiguration<TEndpoint> WithEndpointLogger<TRequest, TResponse>() where TRequest : notnull, new() where TResponse : notnull, new()
    {
        return WithInjectedServices(t => t.AddSingleton(
            new Mock<ILogger<Endpoint<TRequest, TResponse>>>().Object));
    }
    
    /// <summary>
    /// It registers a logger using dependency injection
    /// </summary>
    /// <typeparam name="TRequest">Type of the request</typeparam>
    /// <returns></returns>
    public EndpointConfiguration<TEndpoint> WithEndpointLogger<TRequest>() where TRequest : notnull, new()
    {
        return WithInjectedServices(t => t.AddSingleton(
            new Mock<ILogger<Endpoint<TRequest>>>().Object));
    }
    
    /// <summary>
    /// It registers a logger using dependency injection
    /// </summary>
    /// <typeparam name="TResponse">Type of the response</typeparam>
    /// <returns></returns>
    public EndpointConfiguration<TEndpoint> WithWithResponseOnlyLogger<TResponse>() where TResponse : notnull, new()
    {
        return this.WithInjectedServices(t => t.AddSingleton(
            new Mock<ILogger<EndpointWithoutRequest<TResponse>>>().Object));
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

                if (DependencyInjector.Count is not 0)
                {
                    var services = new ServiceCollection();
                    DependencyInjector.ForEach(t => t.Invoke(services));
                    ctx.RequestServices = services.BuildServiceProvider();
                }
            }, 
            ConstructorParameters
        );

    /// <summary>
    /// It automatically builds the endpoint when casting to it
    /// </summary>
    /// <param name="endpointConfiguration">Endpoint configuration to create the endpoint</param>
    /// <returns>The endpoint object</returns>
    public static implicit operator TEndpoint(EndpointConfiguration<TEndpoint> endpointConfiguration)
        => endpointConfiguration.Build();
}
