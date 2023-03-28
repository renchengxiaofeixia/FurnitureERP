using System.Reflection;

namespace FurnitureERP.Parameters
{
    public class PaginationParamaters
    {
        public int PageNo { get; set; } = 1;
        private int _pageSize = 20;
        const int maxPageSize = 100;
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value >= 1)
                {
                    _pageSize = (value > maxPageSize) ? maxPageSize : value;
                }
            }
        }

        //public static bool TryParse(string context, out PaginationParamaters parameter)
        //{
        //    parameter = new PaginationParamaters();

        //    return true;
        //}

        //public static ValueTask<PaginationParamaters?> BindAsync(HttpContext context, ParameterInfo parameter)
        //{

        //    var pd = new PaginationParamaters
        //    {
        //    };

        //    return ValueTask.FromResult<PaginationParamaters?>(pd);
        //}
    }
}
