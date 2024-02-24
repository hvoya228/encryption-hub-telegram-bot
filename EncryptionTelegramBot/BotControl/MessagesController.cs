using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace EncryptionTelegramBot.BotControl;

public static class MessagesController
{
    private static bool _isEncryptionChosen = false;
    private static bool _isDecryptionChosen = false;
    private static string _encryptionMethod = string.Empty;
    
    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
            return;

        if (message.Text is not { } messageText)
            return;

        if (message.Text is "/start")
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: MessagesContainer.StartMessage,
                cancellationToken: cancellationToken);
            
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: MessagesContainer.PromptExamplesMessage,
                cancellationToken: cancellationToken);
        }
        else if (message.Text is "/help")
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: MessagesContainer.HelpMessage, 
                cancellationToken: cancellationToken);
            
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: MessagesContainer.PromptExamplesMessage,
                cancellationToken: cancellationToken);
        }
        else if (message.Text is "/methods")
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: MessagesContainer.EncryptionMethodsMessage, 
                cancellationToken: cancellationToken);
        }
        else if (message.Text is "/about")
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: MessagesContainer.AboutMessage, 
                cancellationToken: cancellationToken);
        }
        else if (message.Text.Contains("/encrypt"))
        {
            _isEncryptionChosen = true;
            _isDecryptionChosen = false;
            
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "\ud83d\udcac EncryptionHub>Encryption>Method> send me encryption method (check /methods).",  
                cancellationToken: cancellationToken);
        }
        else if (message.Text.Contains("/decrypt"))
        {
            _isDecryptionChosen = true;
            _isEncryptionChosen = false;
            
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "\ud83d\udcac EncryptionHub>Decryption>Method> send me decryption method (check /methods).",  
                cancellationToken: cancellationToken);
        }
        else if(message.Text.Contains('/') && _isEncryptionChosen)
        {
            _isEncryptionChosen = true;
            _isDecryptionChosen = false;
            
            if (!EncryptorsRepository.GetAvailableEncryptors().Contains(message.Text.Replace("/", string.Empty)))
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "\ud83d\uddef EncryptionHub>Encryption>Method>Error> unknown encryption method, use /methods.",  
                    cancellationToken: cancellationToken);
                return;
            }
            
            _encryptionMethod = message.Text.Replace("/", string.Empty);
            
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "\ud83d\udcac EncryptionHub>Encryption>Method>Text> send me a text to encrypt.",  
                cancellationToken: cancellationToken);
        }
        else if(message.Text.Contains('/') && _isDecryptionChosen)
        {
            _isDecryptionChosen = true;
            _isEncryptionChosen = false;
            
            if (!EncryptorsRepository.GetAvailableEncryptors().Contains(message.Text.Replace("/", string.Empty)))
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "\ud83d\uddef EncryptionHub>Encryption>Method>Error> unknown decryption method, use /methods.",  
                    cancellationToken: cancellationToken);
                return;
            }
            
            _encryptionMethod = message.Text.Replace("/", string.Empty);
            
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "\ud83d\udcac EncryptionHub>Decryption>Method>Text> send me a text to decrypt.",  
                cancellationToken: cancellationToken);
        }
        else if (_isEncryptionChosen && _encryptionMethod != string.Empty)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "\ud83d\udcac EncryptionHub>Encryption>Method>Text>Encrypting> Success, result:",  
                cancellationToken: cancellationToken);
            
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: EncryptorsRepository.Encrypt(message.Text, _encryptionMethod),  
                cancellationToken: cancellationToken);
            
            _isEncryptionChosen = false;
            _isDecryptionChosen = false;
            _encryptionMethod = string.Empty;
        }
        else if (_isDecryptionChosen && _encryptionMethod != string.Empty)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "\ud83d\udcac EncryptionHub>Decryption>Method>Text>Decrypting> Success, result:",  
                cancellationToken: cancellationToken);
            
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: EncryptorsRepository.Decrypt(message.Text, _encryptionMethod),  
                cancellationToken: cancellationToken);
            
            _isDecryptionChosen = false;
            _isEncryptionChosen = false;
            _encryptionMethod = string.Empty;
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "\ud83d\uddef EncryptionHub>Error> unknown command, use /help.",  
                cancellationToken: cancellationToken);
        }
    }

    public static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
}