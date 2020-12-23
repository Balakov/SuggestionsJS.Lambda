using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuggestionsJS
{
    public static partial class Methods
    {
        public class SetAllowedTypeRequest
        {
            public string Type { get; set; }
            public bool Enabled { get; set; }
        }

        // { "function": "setAllowedType", "args": { "Type": "Person", "Enabled" : true } }
        public static async Task SetAllowedTypeAsync(System.Text.Json.JsonElement jsonArgs)
        {
            var request = jsonArgs.ToObject<SetAllowedTypeRequest>();

            var allowedTypesTable = Table.LoadTable(new AmazonDynamoDBClient(), "SuggestionsJS_AllowedType");

            Document doc = new Document();
            doc["Type"] = request.Type;
            doc["Enabled"] = request.Enabled;

            await allowedTypesTable.PutItemAsync(doc);
        }
    }
}
