using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Logging;
using WoT__1_DWP_.config;

namespace WoT__1_DWP_
{
    internal class Program
    {
        private static DiscordClient Client { get; set; }
        private static CommandsNextExtension Commands { get; set; }

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
            };

            Client = new DiscordClient(discordConfig);

            Client.Ready += Client_Ready;


            Client.GuildMemberUpdated += async (s, e) =>
            {
                var addedRoles = e.Guild.Roles.Values.Where(r => !e.RolesBefore.Contains(r) && e.RolesAfter.Contains(r));
                var removedRoles = e.Guild.Roles.Values.Where(r => e.RolesBefore.Contains(r) && !e.RolesAfter.Contains(r));

                var channel = await Client.GetChannelAsync(config.channel_id);

                foreach (var role in addedRoles)
                    await channel.SendMessageAsync($"{e.Member.DisplayName} dostał rangę: {role.Name}");

                foreach (var role in removedRoles)
                    await channel.SendMessageAsync($"{e.Member.DisplayName} stracił rangę: {role.Name}");

            };

            /*  Remove warning  TO DO  */
            


            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
