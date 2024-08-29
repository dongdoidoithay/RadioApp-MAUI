
namespace RadioApp.Utils;

/// <summary>
/// Network requesting tool
/// </summary>
public class HttpClientHelper
{
    private static HttpClient _httpClient;

    /// <summary>
    /// initialization
    /// </summary>
    public HttpClientHelper()
    {
        _httpClient = new HttpClient();
    }

    /// <summary> 
    /// Send a GET request 
    /// </summary> 
    /// <param name="url">Requested URL</param> 
    /// <returns>Returns the string obtained by the server request</returns>
    public async Task<string> GetReadString(string url)
    {
        return await _httpClient.GetStringAsync(url);
    }

    /// <summary> 
    /// Send a GET request 
    /// </summary> 
    /// <param name="url">Requested URL</param> 
    /// <returns>Returns the byte array obtained by the server request</returns>
    public async Task<byte[]> GetReadByteArray(string url)
    {
        return await _httpClient.GetByteArrayAsync(url);
    }

    /// <summary> 
    /// Download the file as a byte array 
    /// </summary> 
    /// <param name="url">Requested URL</param> 
    /// <param name="progress">Download progress (range 0-1)</param> 
    /// <param name="buffer Size">Byte size of the buffer during download</param> 
    /// <returns>Return the byte array requested by the server</returns>
    public async Task<byte[]> GetFileByteArray(string url, IProgress<float> progress = null, int bufferSize = 8192)
    {
        using (var responseMessage = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
        {
            progress?.Report(0);
            var content = responseMessage.Content;
            if (content == null)
            {
                return Array.Empty<byte>();
            }
            long contentLength = content.Headers.ContentLength ?? throw new Exception("Unknown file size");
            using (var responseStream = await content.ReadAsStreamAsync())
            {
                var buffer = new byte[bufferSize];
                int length;
                long downloadLength = 0;
                var bytes = new byte[contentLength];
                while ((length = await responseStream.ReadAsync(buffer, 0, bufferSize)) > 0)
                {
                    Array.Copy(buffer, 0, bytes, downloadLength, length);
                    downloadLength += length;
                    progress?.Report((float)downloadLength / contentLength);
                }
                progress?.Report(1);
                return bytes.ToArray();
            }
        }
    }

    /// <summary> 
    /// Send a form-based Post request 
    /// </summary> 
    /// <param name="url">URL to request</param> 
    /// <param name="data">Form parameters</param> 
    /// <returns>Return the string obtained by the server request</returns>
    public async Task<string> PostFormReadString(string url, IEnumerable<KeyValuePair<string, string>> data)
    {
        var form = new FormUrlEncodedContent(data);
        var response = await _httpClient.PostAsync(url, form);
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary> 
    /// Send a Post request in string form 
    /// </summary> 
    /// <param name="url">URL to request</param> 
    /// <param name="data">String or json string</param> 
    /// <returns>Return the string obtained by the server request</returns>
    public async Task<string> PostStringReadString(string url, string data)
    {
        var sc = new StringContent(data);
        var response = await _httpClient.PostAsync(url, sc);
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary> 
    /// Send a Post request in Json format (using UTF8 encoding) 
    /// </summary> 
    /// <param name="url">URL to request</param> 
    /// <param name="data">String or json string</param> 
    /// <returns>Return the string obtained by the server request</returns>
    public async Task<string> PostJsonReadString(string url, string data)
    {
        var sc = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, sc);
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary> 
    /// Send a form-based Post request 
    /// </summary> 
    /// <param name="url">URL to request</param> 
    /// <param name="data">Form parameters</param> 
    /// <returns>Return the byte array obtained by the server request</returns>
    public async Task<byte[]> PostFormReadByteArray(string url, IEnumerable<KeyValuePair<string, string>> data)
    {
        var form = new FormUrlEncodedContent(data);
        var response = await _httpClient.PostAsync(url, form);
        return await response.Content.ReadAsByteArrayAsync();
    }
}