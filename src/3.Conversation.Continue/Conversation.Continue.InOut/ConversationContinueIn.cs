using Microsoft.Bot.Schema;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

public sealed record class ConversationContinueIn
{
    public ConversationContinueIn(ConversationReference reference, IActivity activity)
    {
        Reference = reference;
        Activity = activity;
    }

    public ConversationReference Reference { get; }

    public IActivity Activity { get; }
}