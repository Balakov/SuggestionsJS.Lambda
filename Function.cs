using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SuggestionsJS
{
    public partial class Functions
    {
        public async Task<object> Call(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
        {
            object response = null;

            if (request == null || request.Body == null)
            {
                response = "No request body";
            }
            else
            {
                var jsonRoot = System.Text.Json.JsonDocument.Parse(request.Body).RootElement;

                if (jsonRoot.TryGetProperty("function", out var functionName))
                {
                    jsonRoot.TryGetProperty("args", out var jsonArgs);

                    try
                    {
                        switch (functionName.ToString())
                        {
                            case "getAllowedTypes":
                                response = await Methods.GetAllowedTypesAsync();
                                break;
                            case "addSuggestion":
                                await Methods.AddSuggestionAsync(jsonArgs);
                                break;
                            case "deleteSuggestion":
                                await Methods.DeleteSuggestionAsync(jsonArgs);
                                break;
                            case "getSuggestion":
                                response = await Methods.GetSuggestionAsync(jsonArgs);
                                break;
                            case "getAllSuggestions":
                                response = await Methods.GetAllSuggestionsAsync(jsonArgs);
                                break;
                            case "getSuggestionCounts":
                                response = await Methods.GetSuggestionCountsAsync();
                                break;
                            case "resetSuggestions":
                                await Methods.ResetSuggestionsAsync();
                                break;
                            case "setAllowedType":
                                await Methods.SetAllowedTypeAsync(jsonArgs);
                                break;
                            case "setShowID":
                                await Methods.SetShowIDAsync(jsonArgs);
                                break;
                            default:
                                response = $"Unknown method '{functionName}'";
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        response = new { Error = e.Message };
                    }
                }
                else
                {
                    response = "No method name specified.";
                }
            }

            return response;
        }
    }
}
