// https://dsharpplus.github.io/articles/basics/bot_account.html
using DSharpPlus;
// https://github.com/ncalc/ncalc
using NCalcAsync;

DiscordClient discord = new DiscordClient(new DiscordConfiguration
{
    // do not store TOKENs (or passwords) in source control
    Token = "YOUR_BOT_TOKEN_HERE",
    TokenType = TokenType.Bot,
});

discord.MessageCreated += async (c, e) =>
{
    Console.WriteLine(e.Message.Content);
    if (e.Message.Author.IsBot)
    {
        return;
    }
    if (string.Equals(e.Message.Content, "ping", StringComparison.OrdinalIgnoreCase))
    {
        await e.Message.RespondAsync($"Responding to {e.Message.Author.Username}");
    }
    else if (e.Message.Content.StartsWith("calc", StringComparison.OrdinalIgnoreCase))
    {
        string text = e.Message.Content.Substring("calc ".Length);
        var expression = new Expression(text);
        // if an error happens during expression evaluation, it will be handled in catch statement
        try
        {
            var result = await expression.EvaluateAsync();
            await e.Message.RespondAsync($"Vrednost {text}={result}");
        }
        catch (Exception ex)
        {
            // in case of error, a proper response is sent
            await e.Message.RespondAsync($"Napaka pri računanju {text}={ex.Message}");
        }
    }
};

Console.WriteLine("Connecting");
await discord.ConnectAsync();
Console.WriteLine("Connected");
await Task.Delay(-1);