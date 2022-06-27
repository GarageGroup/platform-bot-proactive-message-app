using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

partial class MessageSendFunc
{
    public ValueTask<Result<Unit, MessageSendFailure>> InvokeAsync(
        MessageSendIn input, CancellationToken cancellationToken = default)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .HandleCancellation()
        .Pipe(
            ExtractConversationRequestOrFailure)
        .ForwardValue(
            conversationGetFunc.InvokeAsync,
            MapConversationGetFailure)
        .MapSuccess(
            @out => new ConversationContinueIn(
                reference: @out.Reference,
                activity: input.Activity))
        .ForwardValue(
            conversationContinueFunc.InvokeAsync,
            static failure => new(MessageSendFailureCode.Unknown, failure.FailureMessage));

    private static Result<ConversationGetIn, MessageSendFailure> ExtractConversationRequestOrFailure(MessageSendIn input)
    {
        if (string.IsNullOrEmpty(input?.Activity?.ChannelId))
        {
            return new MessageSendFailure(MessageSendFailureCode.InvalidActivity, "ChannelId must be specified");
        }

        if (string.IsNullOrEmpty(input?.Activity?.Recipient?.Id))
        {
            return new MessageSendFailure(MessageSendFailureCode.InvalidActivity, "RecipientId must be specified");
        }

        return new ConversationGetIn(
            channelId: input.Activity.ChannelId,
            userId: input.Activity.Recipient.Id);
    }

    private static MessageSendFailure MapConversationGetFailure(ConversationGetFailure failure)
        =>
        failure.FailureCode switch
        {
            ConversationGetFailureCode.ConversationNotFound
            =>
            new(MessageSendFailureCode.ConversationNotFound, failure.FailureMessage),

            _
            =>
            new(MessageSendFailureCode.Unknown, failure.FailureMessage)
        };
}