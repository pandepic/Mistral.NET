using System;
using System.Collections.Generic;
using MistralNET.Chat.DTO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MistralNET.Chat;

public class ChatAPI : BaseMistralAPI
{
    public const string API_PATH = "https://api.mistral.ai/v1/chat/completions";

    public ChatAPI(string apiKey) : base(apiKey) { }

    public async Task<ChatAPIResponse?> GetRawResponseAsync(
        ChatAPIRequest request,
        TimeSpan? timeout = null,
        JsonSerializerSettings? serializerSettings = null)
    {
        var response = await PostRequestAsync<ChatAPIResponse>(
            API_PATH,
            request,
            new Dictionary<string, string>()
            {
                { "Authorization", $"Bearer {APIKey}" },
            },
            timeout,
            serializerSettings);

        return response;
    }
}
