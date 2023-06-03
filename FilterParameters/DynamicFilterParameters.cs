using FurnitureERP.Enums;
using System.Linq.Dynamic.Core;

namespace FurnitureERP.FilterParameters
{
    public static class DynamicFilterParameters
    {
        public static IQueryable<T> JoinDynamicParameter<T>(this IQueryable<T> source,List<SearchParam> searchParams)
        {
            if (searchParams is not null)
            {
                searchParams.ForEach(item =>
                {
                    if (item.FieldType == SearchParamEnum.Contains && !string.IsNullOrEmpty(item.FieldValue))
                    {
                        source = source.Where($"{item.FieldName}.Contains(@0)", item.FieldValue);
                    }
                    if (item.FieldType == SearchParamEnum.Equals && !string.IsNullOrEmpty(item.FieldValue))
                    {
                        source = source.Where($"{item.FieldName} == @0", item.FieldValue);
                    }
                });
            }

            return source;
        }
    }


    public class SearchParam
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SearchParamEnum FieldType { get; set; }
    }
}
