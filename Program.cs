using Discord;
using Discord.WebSocket;
using Discord.Commands;
using KK2.Bot.Services;

namespace KK2.Bot
{
    // This is a minimal example of using Discord.Net's command
    // framework - by no means does it show everything the framework
    // is capable of.
    //
    // You can find samples of using the command framework:
    // - Here, under the 02_commands_framework sample
    // - https://github.com/foxbot/DiscordBotBase - a bare-bones bot template
    // - https://github.com/foxbot/patek - a more feature-filled bot, utilizing more aspects of the library
    public class Program
    {
        // There is no need to implement IDisposable like before as we are
        // using dependency injection, which handles calling Dispose for us.
        private static void Main(string[] args)
            => new Program().MainAsync(args).GetAwaiter().GetResult();

        public async Task MainAsync(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }
            // You should dispose a service provider created using ASP.NET
            // when you are finished using it, at the end of your app's lifetime.
            // If you use another dependency injection framework, you should inspect
            // its documentation for the best way to do this.
            using var services = ConfigureServices();
            var client = services.GetRequiredService<DiscordSocketClient>();

            client.Log += LogAsync;
            services.GetRequiredService<CommandService>().Log += LogAsync;

            // Tokens should be considered secret data and never hard-coded.
            // We can read from the environment variable to avoid hardcoding.
            string botToken = Environment.GetEnvironmentVariable("KK2_BOT_TOKEN") ?? string.Empty;
            ulong botServerChannel = ulong.Parse(Environment.GetEnvironmentVariable("KK2_BOT_SERVER_CHANNEL"));
            ulong botGameChannel = ulong.Parse(Environment.GetEnvironmentVariable("KK2_BOT_GAME_CHANNEL"));
            await client.LoginAsync(TokenType.Bot, botToken);
            await client.StartAsync();

            // Here we initialize the logic required to register our commands.
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            app.MapGet("/message/server/{msg}", (string msg) =>
            {
                _ = (client.GetChannel(botServerChannel) as IMessageChannel).SendMessageAsync(msg);
                return Results.Ok();
            });

            app.MapGet("/message/game/{msg}", (string msg) =>
            {
                _ = (client.GetChannel(botGameChannel) as IMessageChannel).SendMessageAsync(msg);
                return Results.Ok();
            });

            app.Run();
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());

            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }
    }
}