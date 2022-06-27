namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

public sealed record class ConversationGetIn
{
    public ConversationGetIn(string channelId, string userId)
    {
        ChannelId = channelId ?? string.Empty;
        UserId = userId ?? string.Empty;
    }

    public string ChannelId { get; }

    public string UserId { get; }
}