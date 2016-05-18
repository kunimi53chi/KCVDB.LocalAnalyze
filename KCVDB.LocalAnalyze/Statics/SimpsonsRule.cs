using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.Statics
{
    /// <summary>
    /// シンプソンの公式
    /// </summary>
    public static class SimpsonsRule
    {
        /// <summary>
        /// 数値積分用のシンプソンの公式
        /// </summary>
        /// <param name="function">f(x)=yの形で表される積分する関数</param>
        /// <param name="xi">分割区間の始点x0</param>
        /// <param name="h">分割区間の幅h</param>
        /// <returns></returns>
        public static double SimpsonsRuleFunction(Func<double, double> function, double xi, double h)
        {
            //参考：http://hooktail.org/computer/index.php?Simpson§

            return (function(xi) + 4.0 * function(xi + h / 2.0) + function(xi + h)) * h / 6.0;
        }
    }
}
