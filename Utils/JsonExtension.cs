using System.Text.Json;

namespace RadioApp.Utils;

/// <summary>
///Json extension tool
/// </summary>
public static class JsonExtension
{
    /// <summary>
    /// Default Json Serializer Options
    /// </summary>
    public static JsonSerializerOptions DefaultOptions { get; set; } = new JsonSerializerOptions();

    /// <summary>
    /// Stringing object
    /// </summary>
    /// <typeparam name="T">type</typeparam>
    /// <param name="json">Input string</param>
    /// <returns>Converted Json object</returns>
    public static T ToObject<T>(this string json)
    {
        return ToObject<T>(json, DefaultOptions);
    }

    /// <summary>
    /// Stringing object
    /// </summary>
    /// <typeparam name="T">type</typeparam>
    /// <param name="json">Input string</param>
    /// <param name="options">JsonSerializerOptions</param>
    /// <returns>Converted Json object</returns>
    public static T ToObject<T>(this string json, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<T>(json, options);
    }

    /// <summary>
    /// Object transfer string
    /// </summary>
    /// <param name="value">Json Object</param>
    /// <returns>Converted string</returns>
    public static string ToJson<T>(this T value)
    {
        return ToJson(value, DefaultOptions);
    }

    /// <summary>
    /// Object transfer string
    /// </summary>
    /// <param name="value">Json Object</param>
    /// <param name="options">JsonSerializerOptions</param>
    /// <returns>Converted string</returns>
    public static string ToJson<T>(this T value, JsonSerializerOptions options)
    {
        return JsonSerializer.Serialize(value, options);
    }
}
