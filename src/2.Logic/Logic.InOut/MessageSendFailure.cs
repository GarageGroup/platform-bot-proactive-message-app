using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

public readonly record struct MessageSendFailure
{
    private readonly string? failureMessage;

    public MessageSendFailure(MessageSendFailureCode failureCode, [AllowNull] string failureMessage)
    {
        FailureCode = failureCode;
        this.failureMessage = string.IsNullOrEmpty(failureMessage) ? null : failureMessage;
    }

    public MessageSendFailureCode FailureCode { get; }

    public string FailureMessage => failureMessage ?? string.Empty;
}