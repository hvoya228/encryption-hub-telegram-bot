namespace EncryptionTelegramBot.BotControl;

public static class MessagesContainer
{
    public const string StartMessage = "\ud83d\udde8" + "\n" + "\n" +
                                       "Hello, I`m encryption bot." + "\n" +
                                       "I can encrypt and decrypt your messages in different approaches.";

    public const string HelpMessage = "\ud83d\udde8" + "\n" + "\n" +
                                      "If you faced with issues, please, contact with bot creator @hvoya228.";

    public const string PromptExamplesMessage = "\ud83d\udde8" + "\n" + "\n" +
                                                "prompt example: /encrypt => /acc => hello world" + "\n" +
                                                "(Encrypting text 'hello world' with Affine Caesar Cipher)" + "\n" +
                                                "prompt example: /decrypt => /acc => xojjs qsbjl" + "\n" +
                                                "(Decrypting text 'xojjs qsbjl' with Affine Caesar Cipher)";
    
    public const string EncryptionMethodsMessage = "\ud83d\udde8" + "\n" + "\n" +  
                                                  "affine caesar cipher => /acc" + "\n" +
                                                  "simplified data encryption standart => /sdes";
    
    public const string AboutMessage = "\ud83d\udde8" + "\n" + "\n" +
                                       "Creator: @hvoya228" + "\n" +
                                       "GitHub: https://github.com/hvoya228/encryption-hub-telegram-bot";
}