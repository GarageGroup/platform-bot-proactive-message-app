using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

partial class ConversationContinueFunc
{
    public ValueTask<Result<Unit, ConversationContinueFailure>> InvokeAsync(
        ConversationContinueIn input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<Unit, ConversationContinueFailure>>(cancellationToken);
        }

        return InnerInvokeAsync(input, cancellationToken);
    }

    private async ValueTask<Result<Unit, ConversationContinueFailure>> InnerInvokeAsync(
        ConversationContinueIn input, CancellationToken cancellationToken)
    {
        var adapter = lazyAdapter.Value;
        await adapter.ContinueConversationAsync(option.BotId, input.Reference, SendAsync, cancellationToken).ConfigureAwait(false);

        return Result.Success<Unit>(default);

        Task SendAsync(ITurnContext turnContext, CancellationToken cancellationToken)
            =>
            turnContext.SendActivityAsync(input.Activity, cancellationToken);
    }
}