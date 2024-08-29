namespace RadioApp.Utils;

/// <summary>
/// Random number help class
/// </summary>
public static class RandomUtils
{
    private static readonly Random MyRandom = new Random();

    /// <summary>
    ///Get a random number of a specified length
    /// </summary>
    /// <param name="length">length</param>
    /// <returns></returns>
    public static string GetOneByLength(int length = 10)
    {
        string val = "";
        for (int i = 0; i < length; i++)
        {
            val = $"{val}{MyRandom.Next(1, 10)}";
        }
        return val;
    }
    /// <summary>
    ///Get a random element from a list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="input"></param>
    /// <returns></returns>
    public static T GetOneFromList<T>(IList<T> input)
    {
        if (input == null || input.Count == 0)
        {
            return default(T);
        }

        var index = MyRandom.Next(0, input.Count);
        return input[index];
    }

    /// <summary>
    ///Get a hexadecimal random number of a specified length (lowercase)
    /// </summary>
    /// <param name="length">length</param>
    /// <returns></returns>
    public static string GetOneHexByLengthToLower(int length = 1)
    {
        return GetOneHexByLengthToUpper(length).ToLower();
    }

    /// <summary>
    ///Get a hexadecimal random number of a specified length (uppercase)
    /// </summary>
    /// <param name="length">length</param>
    /// <returns></returns>
    public static string GetOneHexByLengthToUpper(int length = 1)
    {
        string result = "";
        while (length > 0)
        {
            result += MyRandom.Next(0, 256).ToString("X").PadLeft(2, '0');
            length--;
        }
        return result;
    }
}
