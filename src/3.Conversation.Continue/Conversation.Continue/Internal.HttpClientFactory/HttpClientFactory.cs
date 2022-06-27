using System.Net.Http;
using GGroupp.Infra;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

internal sealed partial class HttpClientFactory : IHttpClientFactory
{
    private readonly ISocketsHttpHandlerProvider socketsHttpHandlerProvider;

    internal HttpClientFactory(ISocketsHttpHandlerProvider socketsHttpHandlerProvider)
        =>
        this.socketsHttpHandlerProvider = socketsHttpHandlerProvider;
}