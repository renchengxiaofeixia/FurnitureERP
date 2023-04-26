using FurnitureERP.Enums;
using FurnitureERP.Models;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection.Metadata;

namespace FurnitureERP.Utils
{
    public class Params
    {
        private static ConcurrentDictionary<string, string> _cache = new ConcurrentDictionary<string, string>();
        private static void ClearCache()
        {
            _cache = new ConcurrentDictionary<string, string>();
        }

        public static void InitParam(IEnumerable<SysParam> pms)
        {
            try
            {
                var merchantGuid = pms.FirstOrDefault()?.MerchantGuid;
                foreach (var paramItem in pms)
                {
                    _cache[$"{merchantGuid}#{paramItem.ParamName}"] = paramItem.ParamValue;
                }
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }
        }

        public static string GetParam(string key, string defv = "")
        {
            if (_cache.ContainsKey(key))
            {
                defv = _cache[key];
            }
            return defv;
        }

        public static T GetParam<T>(string key, T defv)
        {
            var param = GetParam(key, string.Empty);
            if (!string.IsNullOrEmpty(param))
            {
                defv = JsonSerializer.Deserialize<T>(param);
            }
            return defv;
        }

        public static void SaveParam(SysParam param)
        {
            try
            {
                var key = $"{param.MerchantGuid}#{param.ParamName}";
                if (!_cache.ContainsKey(key) || _cache[key] != param.ParamValue)
                {
                    _cache[key] = param.ParamValue;
                }
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }
        }

        public class Trade
        {
            public static bool SkipGoodsAudit(Guid merchantGuid) =>
                GetParam($"{merchantGuid}TradeSkipGoodsAudit", false);
            public static bool SkipFinAudit(Guid merchantGuid) =>
                GetParam($"{merchantGuid}TradeSkipFinAudit", false);
        }

        public class Inventory
        {
            public static InventoryDimensionEnum Dimension(Guid merchantGuid) => 
                GetParam($"{merchantGuid}InventoryDimension", InventoryDimensionEnum.Item);
            public static InventoryReceiveEnum ReceiveMode(Guid merchantGuid) => 
                GetParam($"{merchantGuid}InventoryReceive", InventoryReceiveEnum.Erp);
        }

        public class AfterSale
        {

        }

    }
}
