using FurnitureERP.Enums;

namespace FurnitureERP.Utils
{
    public class SearchParam
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SearchParamEnum FieldType { get; set; }
    }
}
