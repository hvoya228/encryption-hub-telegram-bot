namespace EncryptionTelegramBot.Encryptors;

public static class AffineCaesarCipher
{
    public static string Encrypt(string text)
    {
        return text.Aggregate("", (current, c) => current + EncryptChar(c, 3, 2));
    }
    
    public static string Decrypt(string text)
    {
        return text.Aggregate("", (current, c) => current + DecryptChar(c, 3, 2));
    }
    
    private static char EncryptChar(char ch, int a, int b)
    {
        if (char.IsUpper(ch))
            return (char)(((a * (ch - 'A') + b) % 26) + 'A');
        else if (char.IsLower(ch))
            return (char)(((a * (ch - 'a') + b) % 26) + 'a');
        else
            return ch;
    }
    
    private static char DecryptChar(char ch, int a, int b)
    {
        int aInverse = 0;
        for (int i = 0; i < 26; i++)
        {
            if ((a * i) % 26 == 1)
            {
                aInverse = i;
                break;
            }
        }

        if (char.IsUpper(ch))
            return (char)(((aInverse * (ch - 'A' - b + 26)) % 26) + 'A');
        else if (char.IsLower(ch))
            return (char)(((aInverse * (ch - 'a' - b + 26)) % 26) + 'a');
        else
            return ch;
    }
}