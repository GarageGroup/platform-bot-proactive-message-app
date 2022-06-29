using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

partial class TurnContextExtensions
{
    internal static Task InvokeAsync(this ITurnContext turnContext, IActivity activity, ILogger? logger, CancellationToken cancellationToken)
        =>
        string.IsNullOrEmpty(activity.Id) switch
        {
            true    => turnContext.SendActivityAsync(activity, cancellationToken),
            _       => turnContext.DeleteAndSendAsync(activity, logger, cancellationToken)
        };

    private static Task DeleteAndSendAsync(this ITurnContext context, IActivity activity, ILogger? logger, CancellationToken cancellationToken)
    {
        var deleteTask = context.TryDeleteActivityAsync(activity.Id, logger, cancellationToken);
        
        activity.Id = null;
        var sendTask = context.SendActivityAsync(activity, cancellationToken);

        return Task.WhenAll(deleteTask, sendTask);
    }

    private static async Task TryDeleteActivityAsync(this ITurnContext context, string id, ILogger? logger, CancellationToken cancellationToken)
    {
        try
        {
            await context.DeleteActivityAsync(id, cancellationToken).ConfigureAwait(false);
        }
        catch(ErrorResponseException exception)
        {
            logger?.LogError(exception, "An unexpected exception was thrown when activity {activityId} was deleteing", id);
        }
    }
}