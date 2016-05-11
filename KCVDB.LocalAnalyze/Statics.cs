using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze
{
    /// <summary>
    /// 統計計算用のクラス
    /// </summary>
    public static class Statics
    {
        #region 内部クラス
        /// <summary>
        /// 信頼区間の計算結果
        /// </summary>
        public class CInterval
        {
            /// <summary>
            /// 信頼区間の下限値
            /// </summary>
            public double Lower { get; set; }
            /// <summary>
            /// 信頼区間の中央値
            /// </summary>
            public double Median { get; set; }
            /// <summary>
            /// 信頼区間の上限値
            /// </summary>
            public double Upper { get; set; }
            /// <summary>
            /// 標本数
            /// </summary>
            public int SampleNum { get; set; }
        }
        #endregion

        /// <summary>
        /// ベルヌーイ試行（二項分布）の期待値に対する信頼区間を計算します
        /// </summary>
        /// <param name="probSuccess">成功率の推定値</param>
        /// <param name="sampleNum">標本数</param>
        /// <param name="trimExcess">信頼区間を0以下または1以上で打ち切るオプション。デフォルトではオン</param>
        /// <param name="zValue">z値。デフォルトでは95%信頼区間になります</param>
        /// <returns>信頼区間の推定値</returns>
        public static CInterval ConfidenceIntervalBinomial(double probSuccess, int sampleNum, bool trimExcess = true, double zValue = 1.959964)
        {
            if(sampleNum == 0)
            {
                throw new DivideByZeroException("sampleNumが0です");
            }
            //標準誤差
            var sdError = Math.Sqrt(probSuccess * (1 - probSuccess) / (double)sampleNum);
            var upper = probSuccess + sdError * zValue;
            var lower = probSuccess - sdError * zValue;
            var median = probSuccess;
            if(trimExcess)
            {
                upper = Math.Max(Math.Min(upper, 1.0), 0.0);
                lower = Math.Max(Math.Min(lower, 1.0), 0.0);
                median = Math.Max(Math.Min(median, 1.0), 0.0);
            }

            var result = new CInterval()
            {
                Lower = lower,
                Median = median,
                Upper = upper,
                SampleNum = sampleNum,
            };
            return result;
        }
    }
}
