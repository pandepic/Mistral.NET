using System;
using System.Collections.Generic;
using System.Text;

namespace MistralNET;

public abstract class BaseMistralAPI : BaseHTTPAPI
{
    public readonly string APIKey;
    
    public BaseMistralAPI(string apiKey)
    {
        APIKey = apiKey;
    }
}
