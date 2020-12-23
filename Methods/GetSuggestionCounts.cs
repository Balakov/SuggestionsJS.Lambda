using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuggestionsJS
{
    public static partial class Methods
    {
        public class GetSuggestionCountsResponse
        {
            public Dictionary<string, int> CountsPerType { get; set; } = new Dictionary<string, int>();
        }

        // { "function": "getSuggestionCounts" }
        public static async Task<GetSuggestionCountsResponse> GetSuggestionCountsAsync()
        {
            var response = new GetSuggestionCountsResponse();

            var client = new AmazonDynamoDBClient();
            var currentShowID = await ShowID.GetAsync(client);

            var suggestionsTable = Table.LoadTable(new AmazonDynamoDBClient(), "SuggestionsJS_Suggestion");

            var filterExpression = new Expression();
            filterExpression.ExpressionStatement = "Used = :used";
            filterExpression.ExpressionAttributeValues.Add(":used", false);

            var searchResults = suggestionsTable.Query(new Primitive(currentShowID, false), filterExpression);

            do
            {
                foreach (var result in await searchResults.GetNextSetAsync())
                {
                    var type = result["SuggestionType"].AsString();

                    if (!response.CountsPerType.ContainsKey(type))
                    {
                        response.CountsPerType.Add(type, 0);
                    }

                    response.CountsPerType[type]++;
                }
            } while (!searchResults.IsDone);

            return response;
        }
    }
}
