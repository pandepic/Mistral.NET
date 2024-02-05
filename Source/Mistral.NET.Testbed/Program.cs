using MistralNET.Chat;

Console.WriteLine("Enter your API key:");

var apiKey = Console.ReadLine();

if (string.IsNullOrEmpty(apiKey))
{
    Console.WriteLine("Invalid API key, press any key to exit...");
    Console.ReadKey();
    return;
}

var chatAPI = new ChatAPI(apiKey);
var chatConversation = new ChatConversation(chatAPI, MistralNET.MistralModelType.MistralMedium);

Console.WriteLine($"Test started with model: {chatConversation.ModelType}");

while (true)
{
    var messageString = Console.ReadLine();

    if (!string.IsNullOrEmpty(messageString))
        chatConversation.AddMessage(ChatMessage.FromUser(messageString));

    try
    {
        var nextMessage = await chatConversation.GetNextAssistantMessageAsync();

        Console.WriteLine();
        Console.WriteLine($"Assistant: {nextMessage?.Message}");
        Console.WriteLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed, send an empty message to try again.");
        Console.WriteLine(ex.ToString());
    }
}
