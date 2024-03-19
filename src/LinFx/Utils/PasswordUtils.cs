﻿namespace LinFx.Utils;

public class PasswordUtils
{
    public static string GeneratePassword(int length = 8)
    {
        if (length <= 0)
            throw new ArgumentOutOfRangeException(nameof(length));

        var azu = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        var az = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        var num = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        var list = new List<string>();
        list.AddRange(azu);
        list.AddRange(az);
        list.AddRange(num);
        list = list.Distinct().ToList();

        var str = "";
        var rand = new Random();
        Parallel.For(0, length, i => str += list[rand.Next(list.Count)]);
        return str;
    }
}
