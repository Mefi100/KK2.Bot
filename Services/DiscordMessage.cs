using Newtonsoft.Json;

namespace KK2.Bot.Services
{
    public class DiscordMessage
    {
        public ulong DiscordChannelId { get; set; }
        public string Message { get; set; }

        public DiscordMessage(ulong channel, string message)
        {
            this.DiscordChannelId = channel;
            this.Message = message;
        }

        [JsonConstructor]
        private DiscordMessage()
        {
        }
    }
}