using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuggestionsJS
{
    public static partial class Methods
    {
        public class DeleteSuggestionRequest
        {
            public string ShowID { get; set; }
            public string GUID { get; set; }
        }

        // { "function": "deleteSuggestion", "args": { "ShowID": "102", "GUID" : "1234" } }
        public static async Task DeleteSuggestionAsync(System.Text.Json.JsonElement jsonArgs)
        {
            var args = jsonArgs.ToObject<DeleteSuggestionRequest>();

            var client = new AmazonDynamoDBClient();

            var suggestionsTable = Table.LoadTable(new AmazonDynamoDBClient(), "SuggestionsJS_Suggestion");

            var primaryHash = new Primitive(args.ShowID, false);
            var secondaryHash = new Primitive(args.GUID, false);

            await suggestionsTable.DeleteItemAsync(primaryHash, secondaryHash);
        }
    }
}
