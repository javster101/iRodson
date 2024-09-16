using Discord;
using Discord.WebSocket;
using System;
using System.Text;

public class Program
{

    private static string channel_id = "1285059581956980736";
    private static DiscordSocketClient client;

    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private static async Task RunScheduleMessage(IMessageChannel irodson_channel)
    {
        var tomorrow = DateTime.Now.AddDays(1).ToString("MMMM dd, yyyy");
        var messageBuilder = new StringBuilder();
        messageBuilder.AppendLine(":bell: **Test Reminder** :bell:")
                  .AppendLine($"Hey @everyone! The Slaggers are convening at **6 PM tomorrow** ({tomorrow}).")
                  .AppendLine()
                  .AppendLine("Please react with one of the following emojis to confirm your attendance:")
                  .AppendLine("- :white_check_mark: for **Yes**")
                  .AppendLine("- :grey_question: for **Maybe**")
                  .AppendLine("- :x: for **No**")
                  .AppendLine()
                  .AppendLine("**Action items:**")
                  .AppendLine("- What is our activity?")
                  .AppendLine("- Where are we meeting?");
        var message = await irodson_channel.SendMessageAsync(messageBuilder.ToString());
        await message.AddReactionAsync(new Emoji("✅"));  // Yes
        await message.AddReactionAsync(new Emoji("❓"));  // Maybe
        await message.AddReactionAsync(new Emoji("❌"));  // No
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
        if (irodson_channel == null){
            throw new Exception("IMessageChannel object is null.");
        }

        if (do_schedule)
        {
            await RunScheduleMessage(irodson_channel);
        }
        else
        {
            await Task.Delay(-1);
        }
    }
}
