using Amazon.DynamoDBv2;
using System.Threading.Tasks;

namespace SuggestionsJS
{
    public static partial class Methods
    {
        public class SetShowIDRequest
        {
            public string ShowID { get; set; }
        }

        // { "function": "setShowID", "args": { "ShowID": "101" } }

        public static async Task SetShowIDAsync(System.Text.Json.JsonElement jsonArgs)
        {
            var args = jsonArgs.ToObject<SetShowIDRequest>();

            await ShowID.SetAsync(new AmazonDynamoDBClient(), args.ShowID);
        }
    }
}
