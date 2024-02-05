using System;
using System.Collections.Generic;
using System.Text;

namespace MistralNET.Chat.DTO;

public class ChatAPIRequestMessage
{
    public string role { get; set; }
    public string content { get; set; }
}

public class ChatAPIRequest
{
    public string model { get; set; }
    public List<ChatAPIRequestMessage> messages { get; set; }
    public float temperature { get; set; } = 0.7f; // 0 to 1
    public float top_p { get; set; } = 1f; // 0 to 1
    public int? max_tokens { get; set; } = null;
    public bool stream { get; set; } = false;
    public bool safe_prompt { get; set; } = false;
    public int? random_seed { get; set; } = null;
}
