using EncryptionTelegramBot.Encryptors;

namespace EncryptionTelegramBot.BotControl;

public static class EncryptorsRepository
{
    public static List<string> GetAvailableEncryptors()
    {
        return new List<string>
        {
            "acc",
            "sdes"
        };
    }
    
    public static string Encrypt(string text, string encryptor)
    {
        try
        {
            return encryptor switch
            {
                "acc" => AffineCaesarCipher.Encrypt(text),
                "sdes" => SimplifiedDataEncryptionStandard.Encrypt(text),
                _ => text
            };
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    
    public static string Decrypt(string text, string encryptor)
    {
        try
        {
            return encryptor switch
            {
                "acc" => AffineCaesarCipher.Decrypt(text),
                "sdes" => SimplifiedDataEncryptionStandard.Decrypt(text),
                _ => text
            };
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}