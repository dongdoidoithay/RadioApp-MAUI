using System.Text.RegularExpressions;

namespace RadioApp.Utils;
/// <summary>
/// Regular expression tool class
/// </summary>
public static class RegexUtils
{
    /// <summary>
    /// The specified regular expression specified in the indicator found the matching item in the specified input string。
    /// </summary>
    /// <param name="input">To search for the string of the matching match。</param>
    /// <param name="pattern">The regular expression mode to match。</param>
    /// <returns>true if the regular expression finds a match; false otherwise。</returns>
    public static bool IsMatch(string input, string pattern)
    {
        return Regex.IsMatch(input, pattern);
    }

    /// <summary>
    /// In the specified input string, use the specified replacement string replacement and specified regular expression matching all string。
    /// </summary>
    /// <param name="input">To search for the string of the matching match。</param>
    /// <param name="pattern">The regular expression mode to match。</param>
    /// <param name="replacement">Replace the string。</param>
    /// <returns>A new string that is identical to the input string except that each match of the string has been replaced by the replacement string. If pattern does not match the current instance, this method returns the current instance unchanged.</returns>
    public static string Replace(string input, string pattern, string replacement)
    {
        return Regex.Replace(input, pattern, replacement);
    }

    /// <summary>
    /// Obtain the first match in the input string according to the regular expression
    /// </summary>
    /// <param name="input">To search for the string of the matching match。</param>
    /// <param name="pattern">The regular expression mode to match。</param>
    /// <returns>If you do not match any item, Success = FALSE, otherwise Result will return the first matching</returns>
    public static (bool success, string result) GetFirst(string input, string pattern)
    {
        MatchCollection mc = Regex.Matches(input, pattern);
        if (mc.Count == 0)
        {
            return (false, string.Empty);
        }
        return (true, mc[0].Value);
    }

    /// <summary>
    /// Obtain all the matching in the input string according to the regular expression
    /// </summary>
    /// <param name="input">To search for the string of the matching match。</param>
    /// <param name="pattern">The regular expression mode to match。</param>
    /// <returns>If you do not match any item, return a empty list, otherwise it will return all the items that are matched</returns>
    public static List<string> GetAll(string input, string pattern)
    {
        var result = new List<string>();
        MatchCollection mc = Regex.Matches(input, pattern);
        if (mc.Count == 0)
        {
            return result;
        }
        foreach (Match item in mc)
        {
            result.Add(item.Value);
        }
        return result;
    }

    /// <summary>
    /// Obtain all the matching in the input string according to the regular expression
    /// </summary>
    /// <param name="input">To search for the string of the matching match。</param>
    /// <param name="pattern">The regular expression mode to match。</param>
    /// <returns>If you do not match any item, return a empty list, otherwise it will return all the items that are matched</returns>
    public static List<string> GetOneGroupAllMatch(string input, string pattern)
    {
        var result = new List<string>();
        MatchCollection mc = Regex.Matches(input, pattern);
        if (mc.Count == 0)
        {
            return result;
        }

        foreach (Match m in mc)
        {
            if (m.Groups.Count == 1)
            {
                return result;
            }
            result.Add(m.Groups[1].Value);
        }

        return result;
    }

    /// <summary>
    /// Obtain all the matching in the input string according to the regular expression
    /// </summary>
    /// <param name="input">To search for the string of the matching match。</param>
    /// <param name="pattern">The regular expression mode to match。</param>
    /// <returns>If you do not match any item, Success = FALSE, otherwise Result will return the matching unique group</returns>
    public static (bool success, string result) GetOneGroupInFirstMatch(string input, string pattern)
    {
        MatchCollection mc = Regex.Matches(input, pattern);
        if (mc.Count == 0)
        {
            return (false, string.Empty);
        }

        //This method only matches the only group, so is the result of the matching of the matching of the mandatory verification?
        if (mc[0].Groups.Count != 2)
        {
            return (false, string.Empty);
        }
        return (true, mc[0].Groups[1].Value);
    }

    /// <summary>
    /// Obtain all the matching in the input string according to the regular expression
    /// </summary>
    /// <param name="input">To search for the string of the matching match。</param>
    /// <param name="pattern">The regular expression mode to match。</param>
    /// <returns>If it is not matched to any item, Success = FALSE, otherwise Result will return an IDICTIORY object with its key value of the name of the group</returns>
    public static (bool success, IDictionary<string, string> result) GetMultiGroupInFirstMatch(string input, string pattern)
    {
        var checkGroupPattern = @"\?<[a-zA-Z]*>";
        var groups = GetAll(pattern, checkGroupPattern);
        if (groups.Count == 0)
        {
            return (false, null);
        }

        MatchCollection mc = Regex.Matches(input, pattern);
        if (mc.Count == 0)
        {
            return (false, null);
        }

        //The number of packed packets is inconsistent with the number of packets that need to be found
        if (mc[0].Groups.Count != groups.Count + 1)
        {
            return (false, null);
        }

        IDictionary<string, string> result = new Dictionary<string, string>();
        foreach (string groupName in groups)
        {
            string groupKey = Replace(groupName, "[^a-zA-Z]", "");
            result.Add(groupKey, mc[0].Groups[groupKey].Value);
        }
        return (true, result);
    }
}