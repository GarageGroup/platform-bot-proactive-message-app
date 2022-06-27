using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

public readonly record struct ConversationGetFailure
{
    private readonly string? failureMessage;

    public ConversationGetFailure(ConversationGetFailureCode failureCode, [AllowNull] string failureMessage)
    {
        FailureCode = failureCode;
        this.failureMessage = string.IsNullOrEmpty(failureMessage) ? null : failureMessage;
    }

    public ConversationGetFailureCode FailureCode { get; }

    public string FailureMessage => failureMessage ?? string.Empty;
}