using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuggestionsJS
{
    public static partial class Methods
    {
        public class GetAllowedTypesResponse
        {
            public List<string> AllowedTypes { get; set; } = new List<string>();
        }

        // { "function": "getAllowedTypes" }
        public static async Task<GetAllowedTypesResponse> GetAllowedTypesAsync()
        {
            var response = new GetAllowedTypesResponse();

            var allowedTypesTable = Table.LoadTable(new AmazonDynamoDBClient(), "SuggestionsJS_AllowedType");

            var scanResults = allowedTypesTable.Scan(new ScanFilter());

            do
            {
                foreach (var result in await scanResults.GetNextSetAsync())
                {
                    if (result["Enabled"].AsBoolean())
                    {
                        response.AllowedTypes.Add(result["Type"].AsString());
                    }
                }
            } while (!scanResults.IsDone);

            return response;
        }
    }
}
