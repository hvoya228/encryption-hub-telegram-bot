using EncryptionTelegramBot.Encryptors;

namespace EncryptionTelegramBot.BotControl;

public static class EncryptorsRepository
{
    public static string Encrypt(string text, string encryptor)
    {
        return encryptor switch
        {
            "acc" => AffineCaesarCipher.Encrypt(text),
            _ => text
        };
    }
    
    public static string Decrypt(string text, string encryptor)
    {
        return encryptor switch
        {
            "acc" => AffineCaesarCipher.Decrypt(text),
            _ => text
        };
    }
}