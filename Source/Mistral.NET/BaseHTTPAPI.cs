using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MistralNET;

public class BaseHTTPAPI
{
    protected async Task<T?> GetRequestAsync<T>(
        string path,
        Dictionary<string, string>? headers = null,
        TimeSpan? timeout = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, path);

        if (headers != null)
        {
            foreach (var header in headers)
                request.Headers.Add(header.Key, header.Value);
        }

        return await SendRequestAsync<T>(request, timeout);
    }

    protected async Task<T?> PostRequestAsync<T>(
        string path,
        object parameters,
        Dictionary<string, string>? headers = null,
        TimeSpan? timeout = null,
        JsonSerializerSettings? serializerSettings = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, path);

        if (headers != null)
        {
            foreach (var header in headers)
                request.Headers.Add(header.Key, header.Value);
        }

        var json = JsonConvert.SerializeObject(parameters, serializerSettings);

        request.Headers.Add("accept", "application/json");
        request.Content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json");

        return await SendRequestAsync<T>(request, timeout);
    }

    protected async Task<T?> SendRequestAsync<T>(
        HttpRequestMessage request,
        TimeSpan? timeout = null)
    {
        using var client = new HttpClient();

        if (timeout.HasValue)
            client.Timeout = timeout.Value;

        using var response = await client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        switch ((int)response.StatusCode)
        {
            case 200:
                return JsonConvert.DeserializeObject<T>(content);

            default:
                throw new Exception("API Request Error - "
                                    + ((int)response.StatusCode).ToString()
                                    + " - " + request.RequestUri.ToString()
                                    + ": " + content);
        }
    }
}