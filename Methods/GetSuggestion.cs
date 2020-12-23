using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuggestionsJS
{
    public static partial class Methods
    {
        public class GetSuggestionRequest
        {
            public string Type { get; set; }
        }

        public class GetSuggestionResponse
        {
            public SuggestionToGet Suggestion { get; set; }
        }

        // { "function": "getSuggestion", "args": { "Type" : "Person" } }
        public static async Task<GetSuggestionResponse> GetSuggestionAsync(System.Text.Json.JsonElement jsonArgs)
        {
            var args = jsonArgs.ToObject<GetSuggestionRequest>();

            var response = new GetSuggestionResponse();

            var client = new AmazonDynamoDBClient();
            var currentShowID = await ShowID.GetAsync(client);

            var suggestionsTable = Table.LoadTable(new AmazonDynamoDBClient(), "SuggestionsJS_Suggestion");

            var filterExpression = new Expression();
            filterExpression.ExpressionStatement = "(SuggestionType = :suggestionType) and (Used = :used)";
            filterExpression.ExpressionAttributeValues.Add(":suggestionType", args.Type);
            filterExpression.ExpressionAttributeValues.Add(":used", false);

            var searchResults = suggestionsTable.Query(new Primitive(currentShowID, false), filterExpression);

            List<Document> allUnusedSuggestions = new List<Document>();

            do
            {
                foreach (var result in await searchResults.GetNextSetAsync())
                {
                    allUnusedSuggestions.Add(result);
                }
            } while (!searchResults.IsDone);

            if (allUnusedSuggestions.Count > 0)
            {
                System.Random rand = new System.Random();
                int indexToUse = rand.Next(allUnusedSuggestions.Count);

                var documentToUse = allUnusedSuggestions[indexToUse];

                response.Suggestion = new SuggestionToGet()
                {
                    SuggestionID = documentToUse["SuggestionID"].AsString(),
                    SuggestionText = documentToUse["SuggestionText"].AsString(),
                    SuggestionType = documentToUse["SuggestionType"],
                    Used = documentToUse["Used"].AsBoolean()
                };

                documentToUse["Used"] = true;

                await suggestionsTable.UpdateItemAsync(documentToUse);
            }

            return response;
        }
    }
}
