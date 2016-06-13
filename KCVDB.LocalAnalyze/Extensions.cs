using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze
{
    /// <summary>
    /// LocalAnalyze用の拡張メソッド
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 艦これAPIのsvdata=のプレフィックスを削除
        /// </summary>
        /// <param name="jsonStr">レスポンスボディ</param>
        /// <returns>プレフィックスを削除したレスポンスボディ</returns>
        public static string RemoveKcsapiPrefix(this string jsonStr)
        {
            return jsonStr.Replace("svdata=", "");
        }
    }
}
