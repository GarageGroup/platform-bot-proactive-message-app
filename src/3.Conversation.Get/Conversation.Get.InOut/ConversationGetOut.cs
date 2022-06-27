using Microsoft.Bot.Schema;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

public sealed record class ConversationGetOut
{
    public ConversationGetOut(ConversationReference reference)
        =>
        Reference = reference;

    public ConversationReference Reference { get; }
}