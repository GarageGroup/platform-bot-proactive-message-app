using System;
using GGroupp.Infra.Bot.Builder;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

using IConversationGetFunc = IAsyncValueFunc<ConversationGetIn, Result<ConversationGetOut, ConversationGetFailure>>;

internal sealed partial class ConversationGetFunc : IConversationGetFunc
{
    public static ConversationGetFunc Create(IFunc<IStorageItemReadSupplier> cosmosApiProvider)
        =>
        new(
            cosmosApiProvider ?? throw new ArgumentNullException(nameof(cosmosApiProvider)));

    private readonly IFunc<IStorageItemReadSupplier> cosmosApiProvider;

    internal ConversationGetFunc(IFunc<IStorageItemReadSupplier> cosmosApiProvider)
        =>
        this.cosmosApiProvider = cosmosApiProvider;
}