using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuggestionsJS
{
    public static partial class Methods
    {
        public class GetAllSuggestionsRequest
        {
            public string Type { get; set; }
        }

        public class SuggestionToGet
        {
            public string SuggestionID { get; set; }
            public string SuggestionType { get; set; }
            public bool Used { get; set; }
            public string SuggestionText { get; set; }
        }

        public class GetAllSuggestionsResponse
        {
            public List<SuggestionToGet> Suggestions { get; set; } = new List<SuggestionToGet>();
        }

        // { "function": "getAllSuggestions", "args": { "Type" : "Person" } }
        public static async Task<GetAllSuggestionsResponse> GetAllSuggestionsAsync(System.Text.Json.JsonElement jsonArgs)
        {
            var args = jsonArgs.ToObject<GetAllSuggestionsRequest>();

            var response = new GetAllSuggestionsResponse();

            var client = new AmazonDynamoDBClient();
            var currentShowID = await ShowID.GetAsync(client);

            var suggestionsTable = Table.LoadTable(new AmazonDynamoDBClient(), "SuggestionsJS_Suggestion");

            var filterExpression = new Expression();
            filterExpression.ExpressionStatement = "SuggestionType = :suggestionType";
            filterExpression.ExpressionAttributeValues.Add(":suggestionType", args.Type);

            var searchResults = suggestionsTable.Query(new Primitive(currentShowID, false), filterExpression);

            do
            {
                foreach (var result in await searchResults.GetNextSetAsync())
                {
                    var type = result["SuggestionType"].AsString();

                    response.Suggestions.Add(new SuggestionToGet()
                    {
                        SuggestionID = result["SuggestionID"].AsString(),
                        SuggestionText = result["SuggestionText"].AsString(),
                        SuggestionType = type,
                        Used = result["Used"].AsBoolean()
                    });
                }
            } while (!searchResults.IsDone);

            return response;
        }
    }
}
