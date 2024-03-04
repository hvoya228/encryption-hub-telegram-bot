using System.Text;

namespace EncryptionTelegramBot.Encryptors;

public static class ByteEncryption
{
    public static string Encrypt(string plaintext) 
    {
        var plaintextBytes = Encoding.ASCII.GetBytes(plaintext);
        var encryptedBytes = new byte[plaintextBytes.Length];

        for (var i = 0; i < plaintextBytes.Length; i++) {
            encryptedBytes[i] = (byte)(plaintextBytes[i] + 1);
        }

        return Convert.ToBase64String(encryptedBytes);
    }

    public static string Decrypt(string encryptedText) 
    {
        var encryptedBytes = Convert.FromBase64String(encryptedText);
        var decryptedBytes = new byte[encryptedBytes.Length];

        for (var i = 0; i < encryptedBytes.Length; i++) {
            decryptedBytes[i] = (byte)(encryptedBytes[i] - 1);
        }

        return Encoding.ASCII.GetString(decryptedBytes);
    }
}