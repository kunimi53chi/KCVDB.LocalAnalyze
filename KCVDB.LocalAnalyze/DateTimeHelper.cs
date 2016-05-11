using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace KCVDB.LocalAnalyze
{
    /// <summary>
    /// 日付操作のヘルパー
    /// </summary>
    public static class DateTimeHelper
    {
        static string[] expectedFormats = { "d", "D", "f", "F", "g", "G", "m", "r", "s", "t", "T", "u", "U", "y" };
        /// <summary>
        /// RFC1123形式の日時文字列をDateTimeOffsetに変換し、変換に成功したかどうかを返します。
        /// 変換に失敗した場合例外をスローしません。
        /// </summary>
        /// <param name="rfcDateStr">RFC1123形式の日時文字列</param>
        /// <param name="dateTimeOffset">変換に成功した場合はrfcDateStrに対応する値、失敗した場合はDateTimeOffset.MinValueが格納されます</param>
        /// <returns></returns>
        public static bool TryParseRFC1123(string rfcDateStr, out DateTimeOffset dateTimeOffset)
        {
            return DateTimeOffset.TryParseExact(rfcDateStr, expectedFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out dateTimeOffset);
        }

        /// <summary>
        /// 日本標準時(JST:GMT+9:00)に変換するための拡張メソッド
        /// </summary>
        /// <param name="dateTimeOffset">現地時間のDateTimeOffset</param>
        /// <returns>日本標準時(JST)のDateTimeOffset</returns>
        public static DateTimeOffset ToJst(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToOffset(new TimeSpan(+9, 0, 0));
        }

        /// <summary>
        /// RFC1123形式の日時文字列を日本標準時(JST:GMT+9:00)DateTimeOffsetに変換し、変換に成功したかどうかを返します。
        /// 変換に失敗した場合例外をスローしません。
        /// </summary>
        /// <param name="rfcDateStr">RFC1123形式の日時文字列</param>
        /// <param name="dateTimeOffsetJst">変換に成功した場合はrfcDateStrに対応する値、
        /// 失敗した場合は世界標準時がDateTimeOffset.MinValueとなるような日本標準時が格納されます</param>
        /// <returns></returns>
        public static bool TryParseRFC1123ToJst(string rfcDateStr, out DateTimeOffset dateTimeOffsetJst)
        {
            DateTimeOffset originalDate;
            if(TryParseRFC1123(rfcDateStr, out originalDate))
            {
                dateTimeOffsetJst = originalDate.ToJst();
                return true;
            }
            else
            {
                dateTimeOffsetJst = new DateTimeOffset(1, 1, 1, 9, 0, 0, new TimeSpan(+9, 0, 0));
                return false;
            }
        }
    }
}
