using System.Reflection;

namespace FurnitureERP.Parameters
{
    public class ItemParameters
    {
        public string? Keyword { get; set; }
        public DateTime? StartCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }

        //public static bool TryParse(string value, out ItemParameters? parameter)
        //{
        //    parameter = new ItemParameters();

        //    return true;
        //}

        //public static ValueTask<ItemParameters?> BindAsync(HttpContext context, ParameterInfo parameter)
        //{

        //    var pd = new ItemParameters
        //    {
        //    };

        //    return ValueTask.FromResult<ItemParameters?>(pd);
        //}
    }
}
