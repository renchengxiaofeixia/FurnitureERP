using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Utils
{
    public class Pagination<T>
    {
        public int PageNo { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalItems { get; private set; } = 0;
        public object Items { get; set; }
        public Pagination(List<T> items, int count, int pageNo, int pageSize, int totalItems)
        {
            PageNo = pageNo;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalItems = totalItems;
            Items = items;
        }
        public bool HasPreviousPage => PageNo > 1;
        public bool HasNextPage => PageNo < TotalPages;
        public static async Task<Pagination<T>> CreateAsync(IQueryable<T> source, int pageIndex = 1, int pageSize = 50)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new Pagination<T>(items, count, pageIndex, pageSize, count);
        }
    }
}
