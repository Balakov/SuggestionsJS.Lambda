
namespace SuggestionsJS
{
    static class JSONArgsConverterExtensionMethods
    {
        public static T ToObject<T>(this System.Text.Json.JsonElement element)
        {
            var json = element.GetRawText();
            return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }
    }
}
