using System;
using System.Text.RegularExpressions;

namespace Chris.Personnel.Management.Common.Extensions
{
    public static class ValidateMethodExtension
    {
        public static bool IsEmail(this string str)
        {
            return Regex.IsMatch(str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public static bool IsVehicleCarNumber(this string str)
        {
            return Regex.IsMatch(str, @"^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}[A-Z0-9]{4,5}[A-Z0-9挂学警港澳]{1}$");
        }

        public static bool IsTelephone(this string str)
        {
            return Regex.IsMatch(str, @"^(0[1-9]{2})-\d{8}$|^(0[1-9]{3} -(\d{7,8}))$|^1[0-9]{10}$");
        }

        public static bool IsCellphone(this string str)
        {
            return Regex.IsMatch(str, @"^0\d{2,3}\d{7,8}$");
        }

        public static bool IsTaxNumber(this string str)
        {
            return Regex.IsMatch(str, @"^[A-Z0-9]{15}$|^[A-Z0-9]{17}$|^[A-Z0-9]{18}$|^[A-Z0-9]{20}$");
        }

        public static bool IsIp(this string str)
        {
            return Regex.IsMatch(str, @"^((25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))$");
        }

        public static bool IsPostCode(this string str)
        {
            return Regex.IsMatch(str, @"^[0-9]{6}$");
        }

        public static bool IsFrameNumber(this string str)
        {
            return Regex.IsMatch(str, @"^([A-Z0-9]{17})$");
        }

        public static bool IsDate(this string str)
        {
            return Regex.IsMatch(str, @"(\d{4})-(\d{1,2})-(\d{1,2})");
        }

        public static bool IsNumeric(this string str)
        {
            return Regex.IsMatch(str, @"^[-]?[0-9]+(\.[0-9]+)?$");
        }

        public static bool IsIdCard(this string str)
        {
            return Regex.IsMatch(str, @"^[1-9]\d{16}[\dXx]$");
        }

        /// <summary>
        /// 是否是图片文件名
        /// </summary>
        /// <returns> </returns>
        public static bool IsImgFileName(this string fileName)
        {
            if (fileName.IndexOf(".", StringComparison.Ordinal) == -1)
                return false;

            var tempFileName = fileName.Trim().ToLower();
            var extension = tempFileName.Substring(tempFileName.LastIndexOf(".", StringComparison.Ordinal));
            return extension == ".png" || extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif";
        }

        #region 空判断
        public static bool IsNullOrEmpty(this string inputStr)
        {
            return string.IsNullOrEmpty(inputStr);
        }

        public static bool IsNullOrWhiteSpace(this string inputStr)
        {
            return string.IsNullOrWhiteSpace(inputStr);
        }

        public static string Format(this string inputStr, params object[] obj)
        {
            return string.Format(inputStr, obj);
        }
        #endregion
    }
}
