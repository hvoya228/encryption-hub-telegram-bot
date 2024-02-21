using EncryptionTelegramBot.BotControl;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

var botClient = new TelegramBotClient("7065257082:AAFihwzbaDC4cLvgnhw3CvhY8xv7N2_Oui4");

using CancellationTokenSource cts = new();

ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>()
};

botClient.StartReceiving(
    updateHandler: MessagesController.HandleUpdateAsync,
    pollingErrorHandler: MessagesController.HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();
  
cts.Cancel();