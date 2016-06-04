using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.Event
{
    /// <summary>
    /// 解析中に例外が発生した場合のイベント引数
    /// </summary>
    public sealed class ExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// 解析中に発生した例外
        /// </summary>
        public Exception Exception { get; private set; }

        public ExceptionEventArgs(Exception exception)
        {
            this.Exception = exception;
        }
    }
}
