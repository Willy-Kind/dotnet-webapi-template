using System.Net;

namespace Template.WebApi.Clients;

/// <summary>
/// Represents a stubbed HTTP message handler that can be used for testing purposes.
/// </summary>
public class StubbedHttpMessageHandler : DelegatingHandler
{
    private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _requestDelegate;

    public StubbedHttpMessageHandler(IExternalAnimalApi externalAnimalApi) =>
            _requestDelegate = (request) => Task.FromResult(externalAnimalApi.GetValueAnimals());

    /// <summary>
    /// Initializes a new instance of the <see cref="StubbedHttpMessageHandler"/> class with a fixed response message.
    /// </summary>
    /// <param name="responseMessage">The response message to be returned by the handler.</param>
    public StubbedHttpMessageHandler(HttpResponseMessage responseMessage) =>
        _requestDelegate = (request) => Task.FromResult(responseMessage);

    /// <summary>
    /// Initializes a new instance of the <see cref="StubbedHttpMessageHandler"/> class with a custom request delegate.
    /// </summary>
    /// <param name="requestDelegate">The delegate that handles the HTTP request and returns the response message.</param>
    public StubbedHttpMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> requestDelegate) =>
        _requestDelegate = requestDelegate;

    /// <inheritdoc/>
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
        _requestDelegate.Invoke(request);
}