using System.Threading.Tasks;
using Discord.Commands;
using KK2.Bot.Services;
using Newtonsoft.Json;

namespace KK2.Bot.Modules
{
    // Modules must be public and inherit from an IModuleBase
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        private const string ServerChannel = "discord-bot-sub";

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