using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MistralNET.Chat.DTO;
using MistralNET.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MistralNET.Chat;

public enum ChatMessageRole
{
    System,
    User,
    Assistant,
}

public class ChatMessage
{
    public ChatMessageRole Role { get; set; }
    public string Message { get; set; } = null!;
    public int? Tokens { get; set; }
    public decimal? EstimatedCost { get; set; }

    public ChatMessage() { }

    public ChatMessage(ChatMessageRole role, string message)
    {
        Role = role;
        Message = message;
    }

    public ChatAPIRequestMessage CreateAPIRequestMessage()
    {
        return new()
        {
            role = EnumUtility.GetChatMessageRoleString(Role),
            content = Message,
        };
    }

    public static ChatMessage FromSystem(string message) => new ChatMessage(ChatMessageRole.System, message);
    public static ChatMessage FromUser(string message) => new ChatMessage(ChatMessageRole.User, message);
    public static ChatMessage FromAssistant(string message) => new ChatMessage(ChatMessageRole.Assistant, message);
}

public class ChatConversation
{
    public ChatAPI ChatAPI { get; protected set; }
    public MistralModelType ModelType { get; protected set; }
    public List<ChatMessage> Messages { get; set; } = new();
    public int? TotalTokens { get; set; }

    public float Temperature { get; protected set; } = 0.7f;
    public float TopP { get; protected set; } = 1f;

    public ChatConversation(ChatAPI chatAPI, MistralModelType modelType)
    {
        ChatAPI = chatAPI;
        ModelType = modelType;
    }

    public void AddMessage(ChatMessage message)
    {
        Messages.Add(message);
    }

    public MistralModel GetModel()
    {
        return MistralModel.Models[ModelType];
    }

    public void SetTemperature(float temperature)
    {
        if (temperature < 0f || temperature > 1f)
            throw new ArgumentException($"Temperature must be between 0 and 1", nameof(temperature));

        Temperature = temperature;
    }

    public void SetTopP(float topP)
    {
        if (topP < 0f || topP > 1f)
            throw new ArgumentException($"TopP must be between 0 and 1", nameof(topP));

        TopP = topP;
    }

    public ChatAPIRequest CreateAPIRequest(
        bool safePrompt = false,
        int? maxTokens = null)
    {
        var request = new ChatAPIRequest()
        {
            model = GetModel().ModelString,
            temperature = Temperature,
            top_p = TopP,
            max_tokens = maxTokens,
            messages = new(),
            safe_prompt = safePrompt,
        };

        foreach (var message in Messages)
            request.messages.Add(message.CreateAPIRequestMessage());

        return request;
    }

    public async Task<ChatMessage?> GetNextAssistantMessageAsync(
        bool safePrompt = false,
        int? maxTokens = null,
        TimeSpan? timeout = null)
    {
        var request = CreateAPIRequest(safePrompt, maxTokens);
        var jsonFormatting = Formatting.None;

        ChatMessage? message = null;

#if DEBUG
        jsonFormatting = Formatting.Indented;
#endif

        var serializerSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = jsonFormatting,
            Converters = new List<JsonConverter>() { new StringEnumConverter() },
        };

        var response = await ChatAPI.GetRawResponseAsync(request, timeout, serializerSettings);

        if (response != null
            && response.choices != null
            && response.choices.Count > 0)
        {
            var choice = response.choices[0];
            message = ChatMessage.FromAssistant(choice.message.content);

            Messages.Add(message);

            if (response.usage != null)
            {
                var model = GetModel();

                var promptCost = model.InputPricePer1kTokens * (response.usage.prompt_tokens / 1000m);
                var responseCost = model.OutputPricePer1kTokens * (response.usage.completion_tokens / 1000m);

                message.EstimatedCost = promptCost + responseCost;
                message.Tokens = response.usage.completion_tokens;
            }
        }

        if (response != null && response.usage != null)
            TotalTokens = response.usage.total_tokens;

        return message;
    }
}
