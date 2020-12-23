using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Threading.Tasks;

namespace SuggestionsJS
{
    public static partial class Methods
    {
        public class SuggestionToAdd
        {
            public string Text { get; set; }
            public string Type { get; set; }
        }

        public class AddSuggestionRequest
        {
            public SuggestionToAdd[] Suggestions { get; set; }
        }

        // { "function": "addSuggestion", "args": { "Suggestions": [{ "Text": "Father Christmas", "Type": "Person" }] } }

        public static async Task AddSuggestionAsync(System.Text.Json.JsonElement args)
        {
            var request = args.ToObject<AddSuggestionRequest>();

            var client = new AmazonDynamoDBClient();

            string showID = await ShowID.GetAsync(client);
            var suggestionsTable = Table.LoadTable(client, "SuggestionsJS_Suggestion");

            var writeBatch = suggestionsTable.CreateBatchWrite();

            foreach (var suggestion in request.Suggestions)
            {
                Document suggestionDocument = new Document();
                suggestionDocument["ShowID"] = showID;
                suggestionDocument["SuggestionID"] = Guid.NewGuid().ToString();
                suggestionDocument["SuggestionType"] = suggestion.Type;
                suggestionDocument["SuggestionText"] = suggestion.Text;
                suggestionDocument["Used"] = false;

                writeBatch.AddDocumentToPut(suggestionDocument);
            }
                
            await writeBatch.ExecuteAsync();
        }
    }
}
