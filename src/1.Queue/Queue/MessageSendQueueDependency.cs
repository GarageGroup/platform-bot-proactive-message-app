using System;
using GGroupp.Infra;
using PrimeFuncPack;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

using IMessageSendFunc = IAsyncValueFunc<MessageSendIn, Result<Unit, MessageSendFailure>>;

public static class MessageSendQueueDependency
{
    public static Dependency<IQueueItemHandler> UseMessageSendQueue(this Dependency<IMessageSendFunc> dependency)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.Map<IQueueItemHandler>(MessageQueueItemHandler.Create);
    }
}