using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

internal static partial class TurnContextExtensions
{
    private static readonly IReadOnlyCollection<string> SupportedDeletingChannels;

    static TurnContextExtensions()
        =>
        SupportedDeletingChannels = new[] { Channels.Telegram, Channels.Msteams };

    private static bool IsChannelSuportedDeleting(this IActivity activity)
        =>
        SupportedDeletingChannels.Contains(activity.ChannelId, StringComparer.InvariantCultureIgnoreCase);
}