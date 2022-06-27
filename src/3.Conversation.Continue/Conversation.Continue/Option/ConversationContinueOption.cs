namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

public sealed record class ConversationContinueOption
{
    public ConversationContinueOption(string botId)
        =>
        BotId = botId ?? string.Empty;

    public string BotId { get; }
}