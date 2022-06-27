using Microsoft.Bot.Schema;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

public sealed record class MessageSendIn
{
    public MessageSendIn(IActivity activity)
        =>
        Activity = activity;

    public IActivity Activity { get; }
}