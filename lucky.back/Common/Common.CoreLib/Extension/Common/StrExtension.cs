namespace Common.CoreLib.Extension.Common
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StrExtension
    {
        /// <summary>
        /// 检测字符串是否包含某值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        public static bool NullCheck(this string? source, string value)
        {
            if (string.IsNullOrEmpty(source)) return false;
            if (string.IsNullOrEmpty(value)) return true; // 空值总是包含于任何字符串？
            return source.Contains(value);
        }
    }
}
