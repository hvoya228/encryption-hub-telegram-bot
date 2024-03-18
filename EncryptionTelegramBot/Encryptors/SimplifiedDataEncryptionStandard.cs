using System.Collections;
using System.Text;

namespace EncryptionTelegramBot.Encryptors;

public static class SimplifiedDataEncryptionStandard
{
    private static readonly BitArray[,] SBox1 = new BitArray[4, 4];
    private static readonly BitArray[,] SBox2 = new BitArray[4, 4];
    private static BitArray _masterKey;
    private const string Key = "1010000010";

    public static string EncryptText(string textToEncrypt)
    {
        GenerateSBoxes();
        return (from c in textToEncrypt from b in Encoding.ASCII.GetBytes(c.ToString()) select Encrypt(b)).Aggregate(string.Empty, (current, encryptedByte) => current + (encryptedByte.ToString() + " "));
    }

    public static string DecryptText(string encryptedText)
    {
        GenerateSBoxes();
        var encryptedBytes = encryptedText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return encryptedBytes.Select(encryptedByte => Decrypt(byte.Parse(encryptedByte))).Aggregate("", (current, decryptedByte) => current + Encoding.ASCII.GetChars(new byte[] { decryptedByte })[0]);
    }

    private static void GenerateSBoxes()
    {
        _masterKey = new BitArray(10);
        for (var i = 0; i < Key.Length; i++)
        {
            _masterKey[i] = Str2Bin(Key[i]);
        }
        
        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                BitArray bits = new(2)
                {
                    [0] = (i == 1 || i == 2),
                    [1] = (j == 2 || j == 3)
                };
                SBox1[i, j] = bits;
            }
        }
        
        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                BitArray bits = new(2)
                {
                    [0] = (i % 3 == 1),
                    [1] = (j % 2 == 0)
                };
                SBox2[i, j] = bits;
            }
        }
    }
    
    private static byte Encrypt(byte block)
    {
        var bitsBlock = Byte2Bits(block);
        var keys = GenerateKeys();
        return Bits2Byte(Rip(Fk(Switch(Fk(Ip(bitsBlock), keys[0])), keys[1])));
    }

    private static byte Decrypt(byte block)
    {
        var bitsBlock = Byte2Bits(block);
        var keys = GenerateKeys();
        return Bits2Byte(Rip(Fk(Switch(Fk(Ip(bitsBlock), keys[1])), keys[0])));
    }

    private static BitArray Byte2Bits(byte block)
    {
        var bits = Decimal2Binstr(block);
        var result = new BitArray(8);
        for (var i = 0; i < bits.Length; i++)
        {
            result[i] = Str2Bin(bits[i]);
        }
        return result;
    }

    private static byte Bits2Byte(BitArray block)
    {
        var result = "";
        for (var i = 0; i < block.Length; i++)
        {
            result += Bin2Str(block[i]);
        }
        return Binstr2decimal(result);
    }

    private static string Decimal2Binstr(byte num)
    {
        var ret = "";
        for (var i = 0; i < 8; i++)
        {
            if (num % 2 == 1)
                ret = "1" + ret;
            else
                ret = "0" + ret;
            num >>= 1;
        }
        return ret;
    }


    private static byte Binstr2decimal(string binstr)
    {
        byte ret = 0;
        foreach (var t in binstr)
        {
            ret <<= 1;
            if (t == '1')
                ret++;
        }
        return ret;
    }

    private static string Bin2Str(bool input)
    {
        if (input)
            return "1";
        else
            return "0";
    }

    private static bool Str2Bin(char bit)
    {
        if (bit == '0')
            return false;
        else if (bit == '1')
            return true;
        else
            throw new Exception("Key should be in binary format [0,1]");
    }

    private static BitArray[] GenerateKeys()
    {
        var keys = new BitArray[2];
        BitArray[] temp = SplitBlock(P10(_masterKey));
        keys[0] = P8(CircularLeftShift(temp[0], 1), CircularLeftShift(temp[1], 1));
        keys[1] = P8(CircularLeftShift(temp[0], 3), CircularLeftShift(temp[1], 3)); 
        return keys;
    }

    private static BitArray P10(BitArray key)
    {
        var permutedArray = new BitArray(10)
        {
            [0] = key[2],
            [1] = key[4],
            [2] = key[1],
            [3] = key[6],
            [4] = key[3],
            [5] = key[9],
            [6] = key[0],
            [7] = key[8],
            [8] = key[7],
            [9] = key[5]
        };

        return permutedArray;
    }

    private static BitArray P8(BitArray part1, BitArray part2)
    {
        var permutedArray = new BitArray(8)
        {
            [0] = part2[0],
            [1] = part1[2],
            [2] = part2[1],
            [3] = part1[3],
            [4] = part2[2],
            [5] = part1[4],
            [6] = part2[4],
            [7] = part2[3]
        };

        return permutedArray;
    }

    private static BitArray P4(BitArray part1, BitArray part2)
    {
        var permutedArray = new BitArray(4)
        {
            [0] = part1[1],
            [1] = part2[1],
            [2] = part2[0],
            [3] = part1[0]
        };

        return permutedArray;
    }

    private static BitArray Ep(BitArray input)
    {
        var permutedArray = new BitArray(8)
        {
            [0] = input[3],
            [1] = input[0],
            [2] = input[1],
            [3] = input[2],
            [4] = input[1],
            [5] = input[2],
            [6] = input[3],
            [7] = input[0]
        };

        return permutedArray;
    }

    private static BitArray Ip(BitArray plainText)
    {
        var permutedArray = new BitArray(8)
        {
            [0] = plainText[1],
            [1] = plainText[5],
            [2] = plainText[2],
            [3] = plainText[0],
            [4] = plainText[3],
            [5] = plainText[7],
            [6] = plainText[4],
            [7] = plainText[6]
        };

        return permutedArray;
    }

    private static BitArray Rip(BitArray permutedText)
    {
        var permutedArray = new BitArray(8)
        {
            [0] = permutedText[3],
            [1] = permutedText[0],
            [2] = permutedText[2],
            [3] = permutedText[4],
            [4] = permutedText[6],
            [5] = permutedText[1],
            [6] = permutedText[7],
            [7] = permutedText[5]
        };

        return permutedArray;
    }

    private static BitArray CircularLeftShift(BitArray a, int bitNumber)
    {
        var shifted = new BitArray(a.Length);
        var index = 0;
        for (var i = bitNumber; index < a.Length; i++)
        {
            shifted[index++] = a[i % a.Length];
        }
        return shifted;
    }

    private static BitArray[] SplitBlock(BitArray block)
    {
        var split = new BitArray[2];
        split[0] = new BitArray(block.Length / 2);
        split[1] = new BitArray(block.Length / 2);
        var index = 0;

        for (var i = 0; i < block.Length / 2; i++)
        {
            split[0][i] = block[i];
        }
        for (var i = block.Length / 2; i < block.Length; i++)
        {
            split[1][index++] = block[i];
        }
        return split;
    }

    private static BitArray SBoxes(BitArray input, int no)
    {
        BitArray[,] currentSBox;

        currentSBox = no == 1 ? SBox1 : SBox2;

        return currentSBox[Binstr2decimal(Bin2Str(input[0]) + Bin2Str(input[3])),
            Binstr2decimal(Bin2Str(input[1]) + Bin2Str(input[2]))];
    }

    private static BitArray F(BitArray right, BitArray sk)
    {
        var temp = SplitBlock(Xor(Ep(right), sk));
        return P4(SBoxes(temp[0], 1), SBoxes(temp[1], 2));
    }

    private static BitArray Fk(BitArray ip, BitArray key)
    {
        var temp = SplitBlock(ip);
        var left = Xor(temp[0], F(temp[1], key));
        var joined = new BitArray(8);
        var index = 0;
        for (var i = 0; i < 4; i++)
        {
            joined[index++] = left[i];
        }
        for (var i = 0; i < 4; i++)
        {
            joined[index++] = temp[1][i];
        }
        return joined;
    }

    private static BitArray Switch(BitArray input)
    {
        var switched = new BitArray(8);
        var index = 0;
        for (var i = 4; index < input.Length; i++)
        {
            switched[index++] = input[i % input.Length];
        }
        return switched;
    }

    private static BitArray Xor(BitArray a, BitArray b)
    {
        return b.Xor(a);
    }
}