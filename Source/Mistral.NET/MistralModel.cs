using System.Collections.Generic;

namespace MistralNET;

public enum MistralModelType
{
    MistralTiny,
    MistralSmall,
    MistralMedium,
    MistralEmbed,
}

public class MistralModel
{
    public MistralModelType ModelType;
    public string ModelString;
    public int MaxTokens;
    public decimal InputPricePer1kTokens; // in EUR
    public decimal OutputPricePer1kTokens; // in EUR
    public int? ResponseTokensPadding;
    public int? TokensPerMessage;
    
    public static readonly Dictionary<MistralModelType, MistralModel> Models = new()
    {
        {
            MistralModelType.MistralTiny,
            new MistralModel()
            {
                ModelType = MistralModelType.MistralTiny,
                ModelString = "mistral-tiny",
                MaxTokens = 32000,
                InputPricePer1kTokens = 0.00014m,
                OutputPricePer1kTokens = 0.00042m,
                ResponseTokensPadding = 6,
                TokensPerMessage = 8,
            }
        },
        {
            MistralModelType.MistralSmall,
            new MistralModel()
            {
                ModelType = MistralModelType.MistralSmall,
                ModelString = "mistral-small",
                MaxTokens = 32000,
                InputPricePer1kTokens = 0.0006m,
                OutputPricePer1kTokens = 0.0018m,
                ResponseTokensPadding = 6,
                TokensPerMessage = 8,
            }
        },
        {
            MistralModelType.MistralMedium,
            new MistralModel()
            {
                ModelType = MistralModelType.MistralMedium,
                ModelString = "mistral-medium",
                MaxTokens = 32000,
                InputPricePer1kTokens = 0.0025m,
                OutputPricePer1kTokens = 0.0075m,
                ResponseTokensPadding = 6,
                TokensPerMessage = 5,
            }
        },
        {
            MistralModelType.MistralEmbed,
            new MistralModel()
            {
                ModelType = MistralModelType.MistralEmbed,
                ModelString = "mistral-embed",
                MaxTokens = 32000,
                InputPricePer1kTokens = 0.0001m,
                OutputPricePer1kTokens = 0m,
            }
        },
    };
}
