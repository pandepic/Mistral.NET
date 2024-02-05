using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MistralNET.Chat;

namespace MistralNET.Utility;

public static class EnumUtility
{
    public static string GetChatMessageRoleString(ChatMessageRole role)
    {
        return role switch
        {
            ChatMessageRole.System => "system",
            ChatMessageRole.User => "user",
            ChatMessageRole.Assistant => "assistant",
            _ => throw new System.NotImplementedException(),
        };
    }
}
