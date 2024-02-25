using MistralNET.Chat.DTO;
using MistralNET.Utility;

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
    public string? Name { get; set; }
    public string Message { get; set; } = null!;
    public int? Tokens { get; set; }
    public decimal? EstimatedCost { get; set; }

    public ChatMessage() { }
    
    public ChatMessage(ChatMessageRole role, string message, string? name = null)
    {
        Role = role;
        Message = message;
        Name = name;
    }
    
    public int GetTokenCount(SharpToken.GptEncoding encoding)
    {
        if (string.IsNullOrEmpty(Message))
            return 0;
        
        return encoding.Encode(Message).Count;
    }

    public ChatAPIRequestMessage CreateAPIRequestMessage()
    {
        return new()
        {
            role = EnumUtility.GetChatMessageRoleString(Role),
            name = Name,
            content = Message,
        };
    }

    public static ChatMessage FromSystem(string message, string? name = null)
        => new ChatMessage(ChatMessageRole.System, message, name);
    public static ChatMessage FromUser(string message, string? name = null)
        => new ChatMessage(ChatMessageRole.User, message, name);
    public static ChatMessage FromAssistant(string message, string? name = null)
        => new ChatMessage(ChatMessageRole.Assistant, message, name);
}