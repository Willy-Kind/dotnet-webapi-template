namespace Template.WebApi.Clients;

public class MockedHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _responseDelegate;

    public MockedHttpMessageHandler(HttpResponseMessage responseMessage)
    {
        _responseDelegate = (request) => Task.FromResult(responseMessage);
    }

    public MockedHttpMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> responseDelegate)
    {
        _responseDelegate = responseDelegate;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return _responseDelegate.Invoke(request);
    }
}