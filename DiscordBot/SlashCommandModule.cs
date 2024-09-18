using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Threading.Tasks;

public class SlashCommandModule : ApplicationCommandModule
{
    [SlashCommand("ping", "Replies with pong!")]
    public async Task PingCommand(InteractionContext ctx)
    {
        // Respond with "pong!" when the /ping command is used
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, 
        new DiscordInteractionResponseBuilder().WithContent("pong!"));
    }
}
