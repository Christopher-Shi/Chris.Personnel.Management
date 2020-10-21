using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Chris.Personnel.Management.Common.Extensions
{
    public static class TypeConvertExtensions
    {
        public static string ToDateTimeString(this DateTime dateTime)
        {
            return $"{dateTime:yyyy-MM-dd HH:mm:ss}";
        }

        public static string ToDateString(this DateTime dateTime)
        {
            return $"{dateTime:yyyy-MM-dd}";
        }

        public static string ToDateTimeString(this DateTime? dateTime)
        {
            return dateTime.HasValue ? $"{dateTime:yyyy-MM-dd HH:mm:ss}" : "";
        }

        public static string ToDateString(this DateTime? dateTime)
        {
            return dateTime.HasValue ? $"{dateTime:yyyy-MM-dd}" : "";
        }

        public static string ToUpperString(this Guid id)
        {
            return id.ToString().ToUpper();
        }

        public static string ToUpperString(this Guid? id)
        {
            return id.HasValue ? id.ToString().ToUpper() : "";
        }

        public static string ToTimeString(this DateTime dateTime)
        {
            return $"{dateTime: HH:mm:ss}";
        }

        public static string ToTimestamp(this DateTime dateTime)
        {
            var ts = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 时间戳转datetime
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(this long seconds)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(seconds);
        }

        public static DateTime ToStartDateTime(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
        }

        public static DateTime ToEndDateTime(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
        }

        public static long To64Int(this string inputStr)
        {
            if (inputStr.IsNullOrEmpty()) return 0;
            long.TryParse(inputStr, out var result);
            return result;
        }

        public static long To32Int(this string inputStr)
        {
            if (inputStr.IsNullOrEmpty()) return 0;
            int.TryParse(inputStr, out var result);
            return result;
        }

        public static bool ToBool(this string inputStr)
        {
            if (inputStr.IsNullOrEmpty()) return false;
            if (!new[] { "true", "false" }.Contains(inputStr.ToLower())) return false;
            return bool.Parse(inputStr);
        }

        #region 字符串截取
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="inputStr">输入</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static string Sub(this string inputStr, int length)
        {
            if (inputStr.IsNullOrEmpty())
                return null;

            return inputStr.Length >= length ? inputStr.Substring(0, length) : inputStr;
        }

        public static string TryReplace(this string inputStr, string oldStr, string newStr)
        {
            return inputStr.IsNullOrEmpty() ? inputStr : inputStr.Replace(oldStr, newStr);
        }

        /// <summary>
        /// example: 删除字符串中的空格: "chris test ".RegexReplace(@"\s", "");
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="pattern"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string RegexReplace(this string inputStr, string pattern, string replacement)
        {
            return inputStr.IsNullOrEmpty() ? inputStr : Regex.Replace(inputStr, pattern, replacement);
        }

        #endregion

        #region 字符格式化
        /// <summary>
        /// 字符格式化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Fmt(this string input, params object[] param)
        {
            if (input.IsNullOrWhiteSpace())
                return null;

            var result = string.Format(input, param);
            return result;
        }

        #endregion

        #region 格式化文本
        /// <summary>
        /// 格式化电话
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static string FmtMobile(this string mobile)
        {
            if (!mobile.IsNullOrEmpty() && mobile.Length > 7)
            {
                var regx = new Regex(@"(?<=\d{3}).+(?=\d{4})", RegexOptions.IgnoreCase);
                mobile = regx.Replace(mobile, "****");
            }

            return mobile;
        }

        /// <summary>
        /// 格式化证件号码
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static string FmtIdCard(this string idCard)
        {
            if (!idCard.IsNullOrEmpty() && idCard.Length > 10)
            {
                var regx = new Regex(@"(?<=\w{6}).+(?=\w{4})", RegexOptions.IgnoreCase);
                idCard = regx.Replace(idCard, "********");
            }

            return idCard;
        }

        /// <summary>
        /// 格式化银行卡号
        /// </summary>
        /// <param name="bankCard"></param>
        /// <returns></returns>
        public static string FmtBankCard(this string bankCard)
        {
            if (!bankCard.IsNullOrEmpty() && bankCard.Length > 4)
            {
                var regx = new Regex(@"(?<=\d{4})\d+(?=\d{4})", RegexOptions.IgnoreCase);
                bankCard = regx.Replace(bankCard, " **** **** ");
            }

            return bankCard;
        }

        #endregion     

        public static string ReverseString(this string input)
        {
            return string.IsNullOrEmpty(input) ? "" : new string(input.ToCharArray().Reverse().ToArray());
        }

        /// <summary>
        /// 16进制byte[]转为string
        /// </summary>
        /// <param name="arrByte"></param>
        /// <returns></returns>
        public static string Byte2String(this byte[] arrByte)
        {
            var sb = new StringBuilder();

            foreach (var b in arrByte)
            {
                sb.Append(b > 15 ? Convert.ToString(b, 16) : '0' + Convert.ToString(b, 16));
            }

            return sb.ToString();
        }
    }
}
