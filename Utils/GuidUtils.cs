
namespace RadioApp.Utils;

/// <summary>
/// GUID tool class
/// </summary>
public static class GuidUtils
{
    /// <summary>
    ///Get a Guid, format: 9af7f46a-ea52-4aa3-b8c3-9fd484c2af12
    /// </summary>
    /// <returns></returns>
    public static string GetFormatDefault()
    {
        return Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Get a GUID, format: E0A953C3EE60EE60EAA9FAE2B667060E09
    /// </summary>
    /// <returns></returns>
    public static string GetFormatN()
    {
        return Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// Get a GUID, format: 9AF7F46A-EA52-4AA3-B8C3-9FD484C2AF12
    /// </summary>
    /// <returns></returns>
    public static string GetFormatD()
    {
        return Guid.NewGuid().ToString("D");
    }

    /// <summary>
    ///Get a Guid, format: {734fd453-a4f8-4c5d-9c98-3fe2d7079760}
    /// </summary>
    /// <returns></returns>
    public static string GetFormatB()
    {
        return Guid.NewGuid().ToString("B");
    }

    /// <summary>
    ///Get a Guid, format: (ade24d16-db0f-40af-8794-1e08e2040df3)
    /// </summary>
    /// <returns></returns>
    public static string GetFormatP()
    {
        return Guid.NewGuid().ToString("P");
    }

    /// <summary>
    ///Get a Guid, format: {0x3fa412e3,0x8356,0x428f,{0xaa,0x34,0xb7,0x40,0xda,0xaf,0x45,0x6f}}
    /// </summary>
    /// <returns></returns>
    public static string GetFormatX()
    {
        return Guid.NewGuid().ToString("X");
    }
}