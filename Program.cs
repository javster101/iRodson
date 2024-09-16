using Discord;
using Discord.WebSocket;

public class Program
{

    private static string channel_id = "1285059581956980736";
    private static DiscordSocketClient client;

    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private static async Task RunScheduleMessage()
    {
    }

    public static async Task Main()
    {
        if (Environment.GetCommandLineArgs().Length != 2)
        {
            System.Console.WriteLine("Usage: dotnet run [bot|schedule]");
            return;
        }

        var command = Environment.GetCommandLineArgs()[1];
        System.Console.WriteLine(command);

        var do_schedule = command switch
        {
            "schedule" => true,
            "bot" => false,
            _ => throw new Exception("Invalid argument. Use 'bot' or 'schedule'."),
        };

        client = new DiscordSocketClient();

        client.Log += Log;

        var token = File.ReadAllText("irodson-key.txt");

        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();

        await Task.Delay(5000);

        var irodson_channel = client.GetChannel(ulong.Parse(channel_id)) as IMessageChannel;

        if (do_schedule)
        {
            await RunScheduleMessage();
        }
        else
        {
            await Task.Delay(-1);
        }
    }
}
