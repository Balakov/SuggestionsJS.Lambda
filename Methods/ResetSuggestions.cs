using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuggestionsJS
{
    public static partial class Methods
    {
        // { "function": "resetSuggestions" }
        public static async Task ResetSuggestionsAsync()
        {
            var client = new AmazonDynamoDBClient();
            var currentShowID = await ShowID.GetAsync(client);

            var suggestionsTable = Table.LoadTable(new AmazonDynamoDBClient(), "SuggestionsJS_Suggestion");

            var searchResults = suggestionsTable.Query(new Primitive(currentShowID, false), new QueryFilter());

            List<Document> allSuggestions = new List<Document>();

            do
            {
                foreach (var result in await searchResults.GetNextSetAsync())
                {
                    allSuggestions.Add(result);
                }
            } while (!searchResults.IsDone);

            if (allSuggestions.Count > 0)
            {
                var batchUpdate = suggestionsTable.CreateBatchWrite();

                foreach (var document in allSuggestions)
                {
                    document["Used"] = false;
                    batchUpdate.AddDocumentToPut(document);
                }

                await batchUpdate.ExecuteAsync();
            }
        }
    }
}
