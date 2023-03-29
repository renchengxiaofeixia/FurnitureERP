﻿
namespace FurnitureERP.Utils
{
    public class ExcelUtil
    {
        /// <summary>
        /// 读取excel的内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fs"> excel文件</param>
        /// <param name="fieldsMapper"></param>
        /// <param name="pictureFieldName"></param>
        /// <param name="tableHeaderRow"></param>
        /// <returns></returns>
        public static (bool, List<T>) TryRead<T>(FileStream fs, Dictionary<string, string> fieldsMapper
            , string pictureFieldName = ""
            , int tableHeaderRow = 4) where T : new()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using var ep = new ExcelPackage(fs);
            var excelWorksheets = ep.Workbook.Worksheets[0];
            var worksheet = excelWorksheets.Cells.Worksheet;

            //var v = worksheet.GetValue(0,0);
            //var vals = (from r in Enumerable.Range(0, 10)
            //from c in Enumerable.Range(0, 7)
            //select new { r, c, v = worksheet.GetValue(r, c) }).ToList();

            var t = typeof(T);
            var props = t.GetProperties().Where(k => fieldsMapper.Values.Any(j => j == k.Name));

            var totalCol = fieldsMapper.Count;
            var totalRow = worksheet.Dimension.Rows;

            var dlist = new List<T>();
            for (var row = 5; row <= totalRow; row++)
            { 
                var item = new T();
                for(var col = 1;col <= totalCol; col++)
                {
                    var fieldName = worksheet.GetValue<string>(tableHeaderRow, col);
                    var propName = fieldsMapper[fieldName];
                    if (fieldName == pictureFieldName) continue;
                    var prop = props.FirstOrDefault(p => p.Name == propName);
                    Type ftyp = prop.PropertyType;
                    //可空类型
                    if (ftyp.IsGenericType && ftyp.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        ftyp = ftyp.GetGenericArguments()[0];
                    }
                    var cellVal = worksheet.GetValue(row, col);
                    var objVal = cellVal == null ? ftyp.GetDefaultValue() : Convert.ChangeType(cellVal, ftyp);
                    prop.SetValue(item, objVal);
                }

                //图片处理
                if (!string.IsNullOrEmpty(pictureFieldName))
                {
                    var pictureColIndex = fieldsMapper.Keys.ToList().IndexOf(pictureFieldName);
                    var propName = fieldsMapper[pictureFieldName];
                    var pic = worksheet.Drawings.FirstOrDefault(p=> p.From.Row == row - 1 && p.From.Column == pictureColIndex) as ExcelPicture;
                    var prop = props.FirstOrDefault(p => p.Name == propName);
                    if (pic != null)
                    {
                        var svrImagePath = Path.Combine(AppContext.BaseDirectory, @"images");
                        var imageName = $"{DateTime.Now.Ticks}.{pic.Image.Type.Value}";
                        var imagePath = Path.Combine(svrImagePath, imageName);
                        File.WriteAllBytes(imagePath, pic.Image.ImageBytes);
                        prop.SetValue(item, $"/images/{imageName}");
                    }                    
                }

                dlist.Add(item);
            }
            return (true,dlist);
        }
    }
}
