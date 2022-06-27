using System;
using GGroupp.Infra.Bot.Builder;
using PrimeFuncPack;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

using IConversationGetFunc = IAsyncValueFunc<ConversationGetIn, Result<ConversationGetOut, ConversationGetFailure>>;

public static class ConversationGetDependency
{
    public static Dependency<IConversationGetFunc> UseConversationGetApi<TCosmosApi>(
        this Dependency<IFunc<TCosmosApi>> dependency)
        where TCosmosApi : class, IStorageItemReadSupplier
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.Map<IConversationGetFunc>(ConversationGetFunc.Create);
    }
}