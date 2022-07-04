using System;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

using IConversationGetFunc = IAsyncValueFunc<ConversationGetIn, Result<ConversationGetOut, ConversationGetFailure>>;
using IConversationContinueFunc = IAsyncValueFunc<ConversationContinueIn, Result<Unit, ConversationContinueFailure>>;

using IMessageSendFunc = IAsyncValueFunc<MessageSendIn, Result<Unit, MessageSendFailure>>;

internal sealed partial class MessageSendFunc : IMessageSendFunc
{
    public static MessageSendFunc Create(IConversationGetFunc conversationGetFunc, IConversationContinueFunc conversationContinueFunc)
    {
        _ = conversationGetFunc ?? throw new ArgumentNullException(nameof(conversationGetFunc));
        _ = conversationContinueFunc ?? throw new ArgumentNullException(nameof(conversationContinueFunc));

        return new(conversationGetFunc, conversationContinueFunc);
    }

    private readonly IConversationGetFunc conversationGetFunc;

    private readonly IConversationContinueFunc conversationContinueFunc;

    private MessageSendFunc(IConversationGetFunc conversationGetFunc, IConversationContinueFunc conversationContinueFunc)
    {
        this.conversationGetFunc = conversationGetFunc;
        this.conversationContinueFunc = conversationContinueFunc;
    }
}