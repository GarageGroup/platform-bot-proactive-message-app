using System;
using PrimeFuncPack;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

using IConversationGetFunc = IAsyncValueFunc<ConversationGetIn, Result<ConversationGetOut, ConversationGetFailure>>;
using IConversationContinueFunc = IAsyncValueFunc<ConversationContinueIn, Result<Unit, ConversationContinueFailure>>;

using IMessageSendFunc = IAsyncValueFunc<MessageSendIn, Result<Unit, MessageSendFailure>>;

public static class MessageSendLogicDependency
{
    public static Dependency<IMessageSendFunc> UseMessageSendLogic(
        this Dependency<IConversationGetFunc, IConversationContinueFunc> dependency)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.Fold<IMessageSendFunc>(MessageSendFunc.Create);
    }
}