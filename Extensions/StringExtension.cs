//
// Summary:
//     Extension method of string type
public static class StringExtension
{
    //
    // Summary:
    //     Whether it is empty
    //
    // Parameters:
    //   input:
    public static bool IsEmpty(this string input)
    {
        return string.IsNullOrEmpty(input);
    }

    //
    // Summary:
    //     Isn't it empty?
    //
    // Parameters:
    //   input:
    public static bool IsNotEmpty(this string input)
    {
        return !string.IsNullOrEmpty(input);
    }

    //
    // Summary:
    //     Convert to a Uri object
    //
    // Parameters:
    //   input:
    public static Uri ToUri(this string input)
    {
        return new Uri(input);
    }
}