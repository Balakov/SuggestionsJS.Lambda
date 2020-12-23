using Amazon.DynamoDBv2;
using System.Threading.Tasks;

namespace SuggestionsJS
{
    public static partial class Methods
    {
        public class GetShowIDResponse
        {
            public string ShowID { get; set; }
        }

        // { "function": "getShowID" }

        public static async Task<GetShowIDResponse> GetShowIDAsync()
        {
            return new GetShowIDResponse()
            {
                ShowID = await ShowID.GetAsync(new AmazonDynamoDBClient())
            };
        }
    }
}
