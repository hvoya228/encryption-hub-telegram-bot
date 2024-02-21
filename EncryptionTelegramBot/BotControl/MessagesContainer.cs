namespace EncryptionTelegramBot.BotControl;

public static class MessagesContainer
{
    public const string StartMessage = "EncryptionHub>Start>" + "\n" + "\n" +
                                       "Hello, I`m encryption bot." + "\n" +
                                       "I can encrypt and decrypt your messages in different approaches.";

    public const string HelpMessage = "EncryptionHub>Help>" + "\n" + "\n" +
                                      "If you faced with issues, please, contact with bot creator @hvoya228.";

    public const string PromptExamplesMessage = "EncryptionHub>Examples>" + "\n" + "\n" +
                                                "prompt example: /encrypt => /acc => hello world" + "\n" +
                                                "(Encrypting text 'hello world' with Affine Caesar Cipher)" + "\n" +
                                                "prompt example: /decrypt => /acc => xojjs qsbjl" + "\n" +
                                                "(Decrypting text 'xojjs qsbjl' with Affine Caesar Cipher)";
    
    public const string EncryptionMethodsMessage = "EncryptionHub>Methods>" + "\n" + "\n" +  
                                                  "affine caesar cipher => /acc" + "\n";
    
    public const string AboutMessage = "EncryptionHub>About>" + "\n" + "\n" +
                                       "Creator: @hvoya228" + "\n" +
                                       "GitHub: https://github.com/hvoya228/encryption-hub-telegram-bot";
}