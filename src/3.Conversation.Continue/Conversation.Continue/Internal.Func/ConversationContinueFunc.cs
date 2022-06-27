using System;
using GGroupp.Infra;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

using IConversationContinueFunc = IAsyncValueFunc<ConversationContinueIn, Result<Unit, ConversationContinueFailure>>;

internal sealed partial class ConversationContinueFunc : IConversationContinueFunc
{
    public static ConversationContinueFunc Create(
        IConfiguration configuration,
        ConversationContinueOption option,
        ISocketsHttpHandlerProvider? handlerProvider,
        ILoggerFactory? loggerFactory)
    {
        _ = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _ = option ?? throw new ArgumentNullException(nameof(option));

        return new(configuration, option, handlerProvider, loggerFactory);
    }

    private readonly Lazy<BotAdapter> lazyAdapter;

    private readonly ConversationContinueOption option;

    private ConversationContinueFunc(
        IConfiguration configuration,
        ConversationContinueOption option,
        ISocketsHttpHandlerProvider? handlerProvider,
        ILoggerFactory? loggerFactory)
    {
        lazyAdapter = new(CreateBotAdapter);
        this.option = option;

        BotAdapter CreateBotAdapter()
            =>
            InnerCreateBotAdapter(configuration, handlerProvider, loggerFactory);
    }

    private static BotAdapter InnerCreateBotAdapter(
        IConfiguration configuration,
        ISocketsHttpHandlerProvider? handlerProvider,
        ILoggerFactory? loggerFactory)
    {
        var authentication = new ConfigurationBotFrameworkAuthentication(
            configuration: configuration,
            httpClientFactory: handlerProvider is null ? null : new HttpClientFactory(handlerProvider),
            logger: loggerFactory?.CreateLogger<ConversationContinueFunc>());

        return new CloudAdapter(authentication);
    }
}