using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

public readonly record struct ConversationContinueFailure
{
    private readonly string? failureMessage;

    public ConversationContinueFailure(ConversationContinueFailureCode failureCode, [AllowNull] string failureMessage)
    {
        FailureCode = failureCode;
        this.failureMessage = string.IsNullOrEmpty(failureMessage) ? null : failureMessage;
    }

    public ConversationContinueFailureCode FailureCode { get; }

    public string FailureMessage => failureMessage ?? string.Empty;
}