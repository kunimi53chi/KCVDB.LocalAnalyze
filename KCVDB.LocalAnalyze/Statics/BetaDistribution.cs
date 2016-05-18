using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.Statics
{
    /// <summary>
    /// ベータ分布
    /// </summary>
    public static class BetaDistribution
    {
        /// <summary>
        /// ベータ関数B(x,y)の値を求めます
        /// B(x,y)=∫[0,1]{t^(x-1) * (1-t)^(y-1)}dt
        /// </summary>
        /// <param name="x">ベータ関数のパラメーターx</param>
        /// <param name="y">ベータ関数のパラメーターy</param>
        /// <param name="numOfSeparation">積分の区切り数</param>
        /// <returns>ベータ関数B(x,y)の値</returns>
        public static double Beta(double x, double y, int numOfSeparation = 1000)
        {
            //y_nを先に計算する
            var yn = new double[2*numOfSeparation+1];
            foreach(var i in Enumerable.Range(0, yn.Length))
            {
                yn[i] = BetaFunctionOfIntegral((double)i / (double)numOfSeparation / 2.0, x, y);
            }
            //積分する関数
            var func = new Func<double, double>((double t) =>
            {
                var index = (int)(t * 2 * numOfSeparation);
                return yn[index];
            });
            //シンプソンの公式で積分する
            double g = 0.0;
            foreach(var i in Enumerable.Range(0, numOfSeparation))
            {
                var ti = (double)i/(double)numOfSeparation;
                g += SimpsonsRule.SimpsonsRuleFunction(func, ti, 1.0 / (double)numOfSeparation);
            }

            return g;
        }

        //ベータ関数の積分の中身
        private static double BetaFunctionOfIntegral(double t, double x, double y)
        {
            return Math.Pow(t, x - 1) * Math.Pow(1 - t, y - 1);
        }

        /// <summary>
        /// ベータ分布の確率密度関数
        /// </summary>
        /// <param name="x">確率変数</param>
        /// <param name="shape1">ベータ関数B(α,β)のパラメーターα</param>
        /// <param name="shape2">ベータ関数B(α,β)のパラメーターβ</param>
        /// <param name="numOfSeparation">積分の区切り数</param>
        /// <returns>ベータ分布の確率密度関数の値</returns>
        public static double DBeta(double x, double shape1, double shape2, int numOfSeparation = 1000)
        {
            return Math.Pow(x, shape1 - 1) * Math.Pow(1 - x, shape2 - 1) / Beta(shape1, shape2, numOfSeparation);
        }

        //ベータ分布のキャッシュ
        internal class BetaDistCache
        {
            internal double Shape1 { get; set; }
            internal double Shape2 { get; set; }
            internal int NumOfSeparation { get; set; }

            //確率密度関数
            private double[] DBetaArray { get; set; }
            //累積分布関数
            private double[] PBetaArray { get; set; }

            internal double[] GetDBetaArray(double shape1, double shape2, int numOfSeparation)
            {
                if(DBetaArray == null ||
                    shape1 != this.Shape1 || shape2 != this.Shape2 || numOfSeparation != this.NumOfSeparation)
                {
                    DBetaArray = new double[2 * numOfSeparation + 1];
                    foreach (var i in Enumerable.Range(0, DBetaArray.Length))
                    {
                        var p = (double)i / (double)numOfSeparation / 2.0;
                        DBetaArray[i] = DBeta(p, shape1, shape2, numOfSeparation);
                    }

                    this.Shape1 = shape1; this.Shape2 = shape2; this.NumOfSeparation = numOfSeparation;
                    this.PBetaArray = null;
                }
                return DBetaArray;
            }

            internal double[] GetPBetaArray(double shape1, double shape2, int numOfSeparation)
            {
                //密度関数の取得
                GetDBetaArray(shape1, shape2, numOfSeparation);
                
                if(PBetaArray == null ||
                    shape1 != this.Shape1 || shape2 != this.Shape2 || numOfSeparation != this.NumOfSeparation)
                {
                    PBetaArray = new double[2 * numOfSeparation + 1];
                    foreach(var i in Enumerable.Range(0, PBetaArray.Length))
                    {
                        var x = (double)i / (double)numOfSeparation / 2.0;
                        PBetaArray[i] = PBeta(x, shape1, shape2, numOfSeparation);
                    }

                    this.Shape1 = shape1; this.Shape2 = shape2; this.NumOfSeparation = numOfSeparation;
                }
                return PBetaArray;
            }
        }

        static BetaDistCache _cache = new BetaDistCache();
        /// <summary>
        /// ベータ分布の累積分布関数
        /// </summary>
        /// <param name="x">確率変数</param>
        /// <param name="shape1">ベータ関数B(α,β)のパラメーターα</param>
        /// <param name="shape2">ベータ関数B(α,β)のパラメーターβ</param>
        /// <param name="numOfSeparation">積分の区切り数</param>
        /// <returns>ベータ分布の累積分布関数の値</returns>
        public static double PBeta(double x, double shape1, double shape2, int numOfSeparation = 1000)
        {
            //確率密度関数を計算する
            var dn = _cache.GetDBetaArray(shape1, shape2, numOfSeparation);
            //積分する関数
            var func = new Func<double, double>((double p) =>
            {
                var index = (int)(p * 2 * numOfSeparation);
                return dn[index];
            });
            //シンプソンの公式で積分する
            double g = 0.0;
            foreach (var i in Enumerable.Range(0, (int)((double)numOfSeparation * x)))
            {
                var ti = (double)i / (double)numOfSeparation;
                g += SimpsonsRule.SimpsonsRuleFunction(func, ti, 1.0 / (double)numOfSeparation);
            }

            return g;
        }

        /// <summary>
        /// ベータ分布の確率点を求める関数
        /// </summary>
        /// <param name="q">確率点</param>
        /// <param name="shape1">ベータ関数B(α,β)のパラメーターα</param>
        /// <param name="shape2">ベータ関数B(α,β)のパラメーターβ</param>
        /// <param name="numOfSeparation">積分の区切り数</param>
        /// <returns>ベータ分布の確率点の値</returns>
        public static double QBeta(double q, double shape1, double shape2, int numOfSeparation = 1000)
        {
            //累積分布関数を計算
            var pn = _cache.GetPBetaArray(shape1, shape2, numOfSeparation);

            //逆関数の探索
            if (q <= pn[0]) return 0.0;
            for (int i = 1; i < pn.Length; i++)
            {
                if (pn[i] >= q)
                {
                    var xi = (double)i / (double)numOfSeparation / 2.0;
                    var xi_1 = (double)(i - 1) / (double)numOfSeparation / 2.0;
                    if(pn[i]-pn[i-1] > 0)
                    {
                        return (q - pn[i - 1]) / (pn[i] - pn[i - 1]) * xi + (pn[i] - q) / (pn[i] - pn[i - 1]) * xi_1;
                    }
                    else
                    {
                        return xi;
                    }
                }
            }
            return 1.0;
        }
    }
}
