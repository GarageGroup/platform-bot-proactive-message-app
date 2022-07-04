using System;
using GGroupp.Infra;
using Newtonsoft.Json;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

using IMessageSendFunc = IAsyncValueFunc<MessageSendIn, Result<Unit, MessageSendFailure>>;

internal sealed partial class MessageQueueItemHandler : IQueueItemHandler
{
    private static readonly JsonSerializerSettings jsonSerializerSettings;

    static MessageQueueItemHandler()
        =>
        jsonSerializerSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

    public static MessageQueueItemHandler Create(IMessageSendFunc messageSendFunc)
        =>
        new(
            messageSendFunc ?? throw new ArgumentNullException(nameof(messageSendFunc)));

    private readonly IMessageSendFunc messageSendFunc;

    private MessageQueueItemHandler(IMessageSendFunc messageSendFunc)
        =>
        this.messageSendFunc = messageSendFunc;
}