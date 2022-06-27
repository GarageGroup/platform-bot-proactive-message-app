using System;
using GGroupp.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PrimeFuncPack;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

using IConversationContinueFunc = IAsyncValueFunc<ConversationContinueIn, Result<Unit, ConversationContinueFailure>>;

public static class ConversationContinueDependency
{
    public static Dependency<IConversationContinueFunc> UseConversationContinueApi(this Dependency<IConfiguration> dependency)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.InnerUseConversationContinueApi();
    }

    private static Dependency<IConversationContinueFunc> InnerUseConversationContinueApi(this Dependency<IConfiguration> dependency)
        =>
        dependency.With(
            sp => sp.GetService<ISocketsHttpHandlerProvider>(),
            sp => sp.GetService<ILoggerFactory>())
        .Fold<IConversationContinueFunc>(
            ConversationContinueFunc.Create);
}