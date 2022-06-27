using System.Net.Http;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

partial class HttpClientFactory
{
    public HttpClient CreateClient(string name)
        =>
        new(
            handler: socketsHttpHandlerProvider.GetOrCreate(name),
            disposeHandler: false);
}