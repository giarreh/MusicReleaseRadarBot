namespace DiscordBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class Worker : BackgroundService
{
       private ILogger<Worker> logger;
        private IConfiguration configuration;
        private DiscordClient discordClient;
        private SlashCommandsExtension slashCommands;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting discord bot");

            string discordBotToken = configuration["DiscordBotToken"];
            discordClient = new DiscordClient(new DiscordConfiguration()
            {
                Token = discordBotToken,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            });

            slashCommands = discordClient.UseSlashCommands();
            slashCommands.RegisterCommands<SlashCommandModule>();

            discordClient.MessageCreated += OnMessageCreated;
            await discordClient.ConnectAsync();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) =>  Task.CompletedTask;

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            discordClient.MessageCreated -= OnMessageCreated;
            await discordClient.DisconnectAsync();
            discordClient.Dispose();
            logger.LogInformation("Discord bot stopped");
        }

        private async Task OnMessageCreated(DiscordClient client, MessageCreateEventArgs e)
        {            
            if (e.Message.Content.StartsWith("ping", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogInformation("pinged, responding with pong!");
                await e.Message.RespondAsync("pong!");
            }
        }

}
