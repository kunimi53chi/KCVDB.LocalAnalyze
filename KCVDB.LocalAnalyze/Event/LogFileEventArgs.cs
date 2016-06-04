using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.Event
{
    /// <summary>
    /// ログファイルの読み込み、解析終了時のイベント引数
    /// </summary>
    public sealed class LogFileEventArgs : EventArgs
    {
        /// <summary>
        /// ログファイル
        /// </summary>
        public LogFile LogFile { get; private set; }

        public LogFileEventArgs
            (LogFile logfile)
        {
            LogFile = logfile;
        }
    }
}
