using System;
using GGroupp.Infra;
using Newtonsoft.Json;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

using IMessageSendFunc = IAsyncValueFunc<MessageSendIn, Result<Unit, MessageSendFailure>>;

internal sealed partial class MessageQueueItemProcessor : IQueueItemProcessor
{
    private static readonly JsonSerializerSettings jsonSerializerSettings;

    static MessageQueueItemProcessor()
        =>
        jsonSerializerSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

    public static MessageQueueItemProcessor Create(IMessageSendFunc messageSendFunc)
        =>
        new(
            messageSendFunc ?? throw new ArgumentNullException(nameof(messageSendFunc)));

    private readonly IMessageSendFunc messageSendFunc;

    private MessageQueueItemProcessor(IMessageSendFunc messageSendFunc)
        =>
        this.messageSendFunc = messageSendFunc;
}