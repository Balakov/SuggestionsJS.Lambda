using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Threading.Tasks;

namespace SuggestionsJS
{
    public class ShowID
    {
        public static async Task<string> GetAsync(IAmazonDynamoDB client)
        {
            var showIDTable = Table.LoadTable(client, "SuggestionsJS_ShowID");

            Primitive showIDHash = new Primitive("Current", false);

            var currentShowIDDocument = await showIDTable.GetItemAsync(showIDHash);

            return currentShowIDDocument?["Value"]?.AsString();
        }

        public static async Task SetAsync(IAmazonDynamoDB client, string showID)
        {
            var showIDTable = Table.LoadTable(client, "SuggestionsJS_ShowID");

            Document newShowIDDocument = new Document();
            newShowIDDocument["ShowID"] = "Current";
            newShowIDDocument["Value"] = showID;
            
            await showIDTable.PutItemAsync(newShowIDDocument);
        }
    }
}
