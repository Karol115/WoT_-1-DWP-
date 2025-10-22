using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using WoT__1_DWP_.config;

namespace WoT__1_DWP_
{
    internal class Program
    {
        private static DiscordClient Client { get; set; }
        //private static CommandsNextExtension Commands { get; set; }

        static async Task Main(string[] args)
        {
            var config = new JsonReader();
            await config.ReadJsonConfig();

            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = config.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Error
            };

            Client = new DiscordClient(discordConfig);

            Client.Ready += Client_Ready;


            Client.GuildMemberUpdated += async (s, e) =>
            {
                var channel = await Client.GetChannelAsync(config.channelId);

                var beforeHighest = e.RolesBefore.Where(r => !config.ignoredRoles.Contains(r.Name)).OrderByDescending(r => r.Position).FirstOrDefault();
                
                var afterHighest = e.RolesAfter.Where(r => !config.ignoredRoles.Contains(r.Name)).OrderByDescending(r => r.Position).FirstOrDefault();

                if (beforeHighest == afterHighest)
                    return;

                var embed = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Blurple,
                    Timestamp = DateTime.Now
                };

                if (afterHighest.Position > beforeHighest.Position)
                {
                    embed.Title = "Awans!";
                    embed.Description = $"{e.Member.DisplayName} awansował do: **{afterHighest.Name}**";
                    embed.Color = DiscordColor.Green;
                }
                else
                {
                    embed.Title = "Degradacja";
                    embed.Description = $"{e.Member.DisplayName} został zdegradowany do: **{afterHighest?.Name ?? "nie wiadomo xddd"}**";
                    embed.Color = DiscordColor.Red;
                }

                await channel.SendMessageAsync(embed: embed.Build());

            };


            await Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
