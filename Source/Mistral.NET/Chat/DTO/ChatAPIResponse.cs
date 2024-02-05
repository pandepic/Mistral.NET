using System;
using System.Collections.Generic;

namespace MistralNET.Chat.DTO;

public class ChatAPIResponseUsage
{
    public int prompt_tokens { get; set; }
    public int completion_tokens { get; set; }
    public int total_tokens { get; set; }
}

public class ChatAPIResponseMessage
{
    public string role { get; set; }
    public string content { get; set; }
}

public class ChatAPIResponseChoice
{
    public int index { get; set; }
    public ChatAPIResponseMessage message { get; set; }
    public string finish_reason { get; set; }
}

public class ChatAPIResponse
{
    public string id { get; set; }
    public string @object { get; set; }
    public long created { get; set; }
    public string model { get; set; }
    public List<ChatAPIResponseChoice> choices { get; set; }
    public ChatAPIResponseUsage usage { get; set; }
}
