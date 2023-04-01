
using Microsoft.Data.SqlClient;
using OfficeOpenXml.Style;
using System.Drawing;

namespace FurnitureERP.Utils
{
    public class Util
    {
        //读取表格
        public static string CheckCellValues(FileStream fs, int totalCol, List<string> cellValues)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using var ep = new ExcelPackage(fs);
            var worksheet = ep.Workbook.Worksheets[0];
            //var worksheet = excelWorksheets.Cells.Worksheet;


            var totalRow = worksheet.Dimension.Rows;

            for (var row = 5; row <= totalRow; row++)               
            {
                for (var col = 1; col <= totalCol; col++)
                {
                    
                    var value = worksheet.GetValue<string>(row, col);
                    if (cellValues.Contains(value)) {
                        worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 0, 0));//设置单元格背景色
                    }
                   

                }
            }

            var svrFn = $"{DateTime.Now.Ticks}.xlsx";
            var svrpath = Path.Combine(AppContext.BaseDirectory, "excel", svrFn);
            ep.SaveAs(svrpath);

            var execlPath = $"/excel/{svrFn}";
            return execlPath;
           
            
        }

        /// <summary>
        /// 读取excel的内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fs"> excel文件</param>
        /// <param name="fieldsMapper"></param>
        /// <param name="pictureFieldName"></param>
        /// <param name="tableHeaderRow"></param>
        /// <returns></returns>
        public static (bool, List<T>) ReadExcel<T>(FileStream fs, Dictionary<string, string> fieldsMapper
            , string pictureFieldName = ""
            , int tableHeaderRow = 4) where T : new()
        {

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using var ep = new ExcelPackage(fs);
            var worksheet = ep.Workbook.Worksheets[0];
            //var worksheet = excelWorksheets.Cells.Worksheet;

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
                for (var col = 1; col <= totalCol; col++)
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
                    var pic = worksheet.Drawings.FirstOrDefault(p => p.From.Row == row - 1 && p.From.Column == pictureColIndex) as ExcelPicture;
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
            return (true, dlist);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="merchantGuid">商户</param>
        /// <param name="modNo">系统模块编号</param>
        /// <returns></returns>
        public static async Task<string> GetSerialNoAsync(AppDbContext db, Guid merchantGuid, string modNo, int endingNumLen = 8)
        {
            var serial = await db.SerialNos.FirstOrDefaultAsync(x=>x.MerchantGuid == merchantGuid && x.ModuleNo == modNo);
            var serialNoParameter = new SqlParameter()
            {
                ParameterName = "OutSerNo",
                SqlDbType = System.Data.SqlDbType.NVarChar,
                Size = 50,
                Direction = System.Data.ParameterDirection.Output
            };
            await db.Database.ExecuteSqlRawAsync("p_getserialno @ModuleNo, @MerchantGuid , @EndingNumLen, @Num, @OutSerNo OUTPUT",
                new SqlParameter("@ModuleNo", modNo),
                new SqlParameter("@MerchantGuid", merchantGuid),
                new SqlParameter("@EndingNumLen", endingNumLen),
                new SqlParameter("@Num", 1),
                serialNoParameter);
            return serialNoParameter.Value.ToString();
        }
    }

}
