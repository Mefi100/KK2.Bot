using Discord.Commands;

namespace KK2.Bot.Modules
{
    // Modules must be public and inherit from an IModuleBase
    public class PublicModule : ModuleBase<SocketCommandContext>
    {

        [Command("hello")]
        public Task HelloAsync()
            => ReplyAsync("Witaj!");

       /* [Command("players")]
        public async Task PlayerInfoAsync()
        {
            await PushToRedisAsync("players");
        }

        [Command("uptime")]
        public async Task ServerUptimeAsync()
        {
            await PushToRedisAsync("uptime");
        } */

    }
}