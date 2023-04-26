using System.Security.Cryptography;

namespace FurnitureERP.Utils
{
    public static class Encrypt
    {
        /// <summary>
        ///     对字符串进行MD5摘要
        /// </summary>
        /// <param name="message">需要摘要的字符串</param>
        /// <returns>MD5摘要字符串</returns>
        public static string MDString(this string message)
        {
            MD5 md5 = MD5.Create();
            byte[] buffer = Encoding.Default.GetBytes(message);
            byte[] bytes = md5.ComputeHash(buffer);
            return GetHexString(bytes);
        }

        /// <summary>
        ///     对字符串进行MD5二次摘要
        /// </summary>
        /// <param name="message">需要摘要的字符串</param>
        /// <returns>MD5摘要字符串</returns>
        public static string MDString2(this string message) => MDString(MDString(message));

        /// <summary>
        /// MD5 三次摘要算法
        /// </summary>
        /// <param name="s">需要摘要的字符串</param>
        /// <returns>MD5摘要字符串</returns>
        public static string MDString3(this string s)
        {
            using MD5 md5 = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            byte[] bytes1 = md5.ComputeHash(bytes);
            byte[] bytes2 = md5.ComputeHash(bytes1);
            byte[] bytes3 = md5.ComputeHash(bytes2);
            return GetHexString(bytes3);
        }

        /// <summary>
        ///     对字符串进行MD5加盐摘要
        /// </summary>
        /// <param name="message">需要摘要的字符串</param>
        /// <param name="salt">盐</param>
        /// <returns>MD5摘要字符串</returns>
        public static string MDString(this string message, string salt) => MDString(message + salt);

        /// <summary>
        ///     对字符串进行MD5二次加盐摘要
        /// </summary>
        /// <param name="message">需要摘要的字符串</param>
        /// <param name="salt">盐</param>
        /// <returns>MD5摘要字符串</returns>
        public static string MDString2(this string message, string salt) => MDString(MDString(message + salt), salt);

        /// <summary>
        /// MD5 三次摘要算法
        /// </summary>
        /// <param name="s">需要摘要的字符串</param>
        /// <param name="salt">盐</param>
        /// <returns>MD5摘要字符串</returns>
        public static string MDString3(this string s, string salt)
        {
            using MD5 md5 = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(s + salt);
            byte[] bytes1 = md5.ComputeHash(bytes);
            byte[] bytes2 = md5.ComputeHash(bytes1);
            byte[] bytes3 = md5.ComputeHash(bytes2);
            return GetHexString(bytes3);
        }

        public static string GetHexString(byte[] bytes)
        {
            var hexArray = new char[bytes.Length << 1];
            for (var i = 0; i < hexArray.Length; i += 2)
            {
                var b = bytes[i >> 1];
                hexArray[i] = GetHexValue(b >> 4);       // b / 16
                hexArray[i + 1] = GetHexValue(b & 0xF);  // b % 16
            }
            return new string(hexArray, 0, hexArray.Length);

            static char GetHexValue(int i)
            {
                if (i < 10)
                {
                    return (char)(i + '0');
                }
                return (char)(i - 10 + 'a');
            }
        }
    }
}
