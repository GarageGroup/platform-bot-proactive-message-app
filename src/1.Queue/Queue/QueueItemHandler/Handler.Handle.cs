using System;
using System.Threading;
using System.Threading.Tasks;
using GGroupp.Infra;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

partial class MessageQueueItemHandler
{
    public ValueTask<Result<Unit, QueueItemFailure>> HandleAsync(QueueItemIn input, CancellationToken cancellationToken = default)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .HandleCancellation()
        .Pipe(
            GetNotEmptyMessageOrFailure)
        .Forward(
            DeserializeActivityOrFailure)
        .Forward(
            NotNullOrFailure)
        .MapSuccess(
            static activity => new MessageSendIn(activity))
        .ForwardValue(
            messageSendFunc.InvokeAsync,
            static failure => failure.FailureCode switch
            {
                MessageSendFailureCode.InvalidActivity => new(failure.FailureMessage, returnToQueue: false),
                _ => new(failure.FailureMessage, returnToQueue: true)
            });

    private static Result<string, QueueItemFailure> GetNotEmptyMessageOrFailure(QueueItemIn? input)
        =>
        string.IsNullOrEmpty(input?.Message) switch
        {
            false => Result.Success(input.Message),
            _ => new QueueItemFailure("Queue message must be specified", returnToQueue: false)
        };

    private static Result<Activity?, QueueItemFailure> DeserializeActivityOrFailure(string message)
    {
        try
        {
            return JsonConvert.DeserializeObject<Activity>(message, jsonSerializerSettings);
        }
        catch(JsonException exception)
        {
            return new QueueItemFailure(exception.Message, returnToQueue: false);
        }
    }

    private static Result<Activity, QueueItemFailure> NotNullOrFailure(Activity? activity)
        =>
        activity switch
        {
            not null => Result.Success(activity),
            _ => new QueueItemFailure("Queue message activity must be specified", returnToQueue: false)
        };
}