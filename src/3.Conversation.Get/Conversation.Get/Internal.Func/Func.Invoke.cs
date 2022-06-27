using System;
using System.Threading;
using System.Threading.Tasks;
using GGroupp.Infra.Bot.Builder;
using Microsoft.Bot.Schema;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

partial class ConversationGetFunc
{
    public ValueTask<Result<ConversationGetOut, ConversationGetFailure>> InvokeAsync(
        ConversationGetIn input, CancellationToken cancellationToken = default)
        =>
        AsyncPipeline.Pipe(
            input ?? throw new ArgumentNullException(nameof(input)), cancellationToken)
        .HandleCancellation()
        .PipeValue(
            InnerInvokeAsync);

    private async ValueTask<Result<ConversationGetOut, ConversationGetFailure>> InnerInvokeAsync(
        ConversationGetIn input, CancellationToken cancellationToken)
    {
        using var cosmosApi = cosmosApiProvider.Invoke();
        return await InnerInvokeAsync(cosmosApi, input, cancellationToken);
    }

    private static ValueTask<Result<ConversationGetOut, ConversationGetFailure>> InnerInvokeAsync(
        IStorageItemReadSupplier cosmosApi, ConversationGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => new StorageItemPath(
                itemType: StorageItemType.UserState,
                channelId: @in.ChannelId,
                userId: @in.UserId))
        .PipeValue(
            cosmosApi.ReadItemAsync)
        .MapFailure<ConversationGetFailure>(
            static failure => failure.FailureCode switch
            {
                StorageItemReadFailureCode.NotFound => new(ConversationGetFailureCode.ConversationNotFound, failure.FailureMessage),
                _ => new(ConversationGetFailureCode.Unknown, failure.FailureMessage)
            })
        .Forward(
            static item => item.Value?.GetProperty<ConversationReference>("__conversationReference") switch
            {
                ConversationReference reference => Result.Success(reference).With<Failure<Unit>>(),
                _ => Failure.Create($"Conversation reference for item {item.Key} is absent")
            },
            static failure => new(ConversationGetFailureCode.ConversationNotFound, failure.FailureMessage))
        .MapSuccess(
            static reference => new ConversationGetOut(
                reference: reference));
}