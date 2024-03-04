using System.Numerics;
using EncryptionTelegramBot.Encryptors;

namespace EncryptionTelegramBot.BotControl;

public static class EncryptorsRepository
{
    public static List<string> GetAvailableEncryptors()
    {
        return new List<string>
        {
            "acc",
            "byte"
        };
    }
    
    public static string Encrypt(string text, string encryptor)
    {
        try
        {
            return encryptor switch
            {
                "acc" => AffineCaesarCipher.Encrypt(text),
                "byte" => ByteEncryption.Encrypt(text),
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
                "byte" => ByteEncryption.Decrypt(text),
                _ => text
            };
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}