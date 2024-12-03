using System.Diagnostics.Eventing.Reader;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
Random r = new Random();

Dictionary<string, string> phoneBook = new Dictionary<string, string>();
bool waitingname = false;
string name ="";
using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("7572627144:AAHBxHM8RzaLYV77qLv0KUBXxOf1EQl8_jM", cancellationToken: cts.Token);
var me = await bot.GetMe();
bot.OnError += OnError;
bot.OnMessage += OnMessage;
bot.OnUpdate += OnUpdate;


Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel(); // stop the bot

// method to handle errors in polling or in your OnMessage/OnUpdate code
async Task OnError(Exception exception, HandleErrorSource source)
{
    Console.WriteLine(exception); // just dump the exception to the console
}

// method that handle messages received by the bot:
async Task OnMessage(Message msg, UpdateType type)
{
    if (msg.Text == "/start")
    {
        bool phoneExist1 = phoneBook.ContainsKey(Convert.ToString(msg.Chat.Id));
        if (phoneExist1)
        {
            await bot.SendMessage(msg.Chat, $"HELLO {name}");
        } else
        {
            await bot.SendMessage(msg.Chat, "enter ur name");
            waitingname = true;
        }
    }   

    else if (msg.Text == "farm")
    {
        await bot.SendMessage(msg.Chat, "farming...");
    }
    else if (msg.Text == "stata")
    {
        await bot.SendMessage(msg.Chat, "check ur stats<3");
        await bot.SendMessage(msg.Chat, $"u have {r.Next(0, 999999)}");
    }
    else if (waitingname)
    {
        name = msg.Text;
        var replyMarkup = new ReplyKeyboardMarkup(true)
             .AddButtons("farm", "stata");
        var send = await bot.SendMessage(msg.Chat, "HELLOOOO!!", replyMarkup: replyMarkup);
        phoneBook.Add(Convert.ToString(msg.Chat.Id), name);
        waitingname = false;
    }
}

// method that handle other types of updates received by the bot:
async Task OnUpdate(Update update)
{
    if (update is { CallbackQuery: { } query }) // non-null CallbackQuery
    {
        await bot.AnswerCallbackQuery(query.Id, $"You picked {query.Data}");
        await bot.SendMessage(query.Message!.Chat, $"User {query.From} clicked on {query.Data}");
    }
}
