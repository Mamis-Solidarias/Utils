using System.Net.Http.Json;
using System.Text.Json;

namespace MamisSolidarias.Utils.Http;


/// <summary>
/// Request builder
/// </summary>
public class ReadyRequest
{
    private readonly HttpClient _client;
    private readonly HttpRequestMessage _requestMessage;

    /// <summary>
    /// It creates a request from an httpclient and a message
    /// </summary>
    /// <param name="client"></param>
    /// <param name="request"></param>
    public ReadyRequest(HttpClient client, HttpRequestMessage request)
    {
        _client = client;
        _requestMessage = request;
    }
    
    /// <summary>
    /// It adds a body to the request
    /// </summary>
    /// <param name="body">Request body</param>
    /// <typeparam name="TRequest">Type of the request body</typeparam>
    public ReadyRequest WithContent<TRequest>(TRequest body)
    {
        _requestMessage.Content = JsonContent.Create(body);
        return this;
    }

    /// <summary>
    /// It executes the http request
    /// </summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>The response, if it is successful</returns>
    /// <exception cref="HttpRequestException">There was a non 2xx response code</exception>
    public async Task<TResponse?> ExecuteAsync<TResponse>(CancellationToken token)
    {
        var response = await _client.SendAsync(_requestMessage, token);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: token);
        
        var msg = await response.Content.ReadAsStringAsync(token);
        throw new HttpRequestException(msg,null,response.StatusCode);
    }
    
    /// <summary>
    /// It executes the http request
    /// </summary>
    /// <param name="token">Cancellation token</param>
    /// <exception cref="HttpRequestException">There was a non 2xx response code</exception>
    public async Task ExecuteAsync(CancellationToken token)
    {
        var response = await _client.SendAsync(_requestMessage, token);
        if (!response.IsSuccessStatusCode)
        {
            var msg = await response.Content.ReadAsStringAsync(token);
            throw new HttpRequestException(msg,null,response.StatusCode);
        }
    }

    /// <summary>
    /// It adds query parameters to the request
    /// </summary>
    /// <param name="parameters">Key-value pairs that will be used as query parameters</param>
    public ReadyRequest WithQuery(params (string Key, string? Value)[] parameters)
    {
        ArgumentNullException.ThrowIfNull(_requestMessage.RequestUri);
        var query = string.Join(
            '&',
            parameters.Where(t => t.Value is not null).Select(t => $"{t.Key}={t.Value}")
        );
        _requestMessage.RequestUri = new Uri(_requestMessage.RequestUri, $"?{query}");
        return this;
    }
}