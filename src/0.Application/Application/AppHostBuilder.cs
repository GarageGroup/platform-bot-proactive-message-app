using System;
using GGroupp.Infra;
using GGroupp.Infra.Bot.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PrimeFuncPack;

namespace GGroupp.Platrom.Bot.ProactiveMessage.Send;

using IConversationGetFunc = IAsyncValueFunc<ConversationGetIn, Result<ConversationGetOut, ConversationGetFailure>>;
using IConversationContinueFunc = IAsyncValueFunc<ConversationContinueIn, Result<Unit, ConversationContinueFailure>>;

internal static class AppHostBuilder
{
    internal static IHostBuilder ConfigureMessageSendQueueProcesor(this IHostBuilder builder)
        =>
        builder.ConfigureQueueProcessor(
            UseMessageSendQueueItemProcesor().Resolve);

    private static Dependency<IQueueItemProcessor> UseMessageSendQueueItemProcesor()
        =>
        UseConversationGetApi().With(
            UseConversationContinueApi())
        .UseMessageSendLogic()
        .UseMessageSendQueue();

    private static Dependency<IConversationGetFunc> UseConversationGetApi()
        =>
        PrimaryHandler.UseStandardSocketsHttpHandler()
        .UseLogging(
            sp => sp.GetRequiredService<ILoggerFactory>().CreateLogger("ProactiveMessageSend"))
        .UseCosmosApi(
            sp => sp.GetConfiguration().GetSection("CosmosApi").GetCosmosApiOption())
        .UseConversationGetApi();

    private static Dependency<IConversationContinueFunc> UseConversationContinueApi()
        =>
        Dependency.From(
            GetConfiguration,
            sp => sp.GetConfiguration().GetConversationContinueOption())
        .UseConversationContinueApi();

    private static IConfiguration GetConfiguration(this IServiceProvider serviceProvider)
        =>
        serviceProvider.GetRequiredService<IConfiguration>();

    private static CosmosApiOption GetCosmosApiOption(this IConfigurationSection section)
        =>
        new(
            baseAddress: new(section.GetValue<string>("BaseAddressUrl")),
            masterKey: section.GetValue<string>("MasterKey"),
            databaseId: section.GetValue<string>("DatabaseId"));

    private static ConversationContinueOption GetConversationContinueOption(this IConfiguration configuration)
        =>
        new(
            botId: configuration.GetValue<string>("MicrosoftAppId"));
}