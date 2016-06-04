using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.Event
{
    /// <summary>
    /// 行単位での解析が行われる際のイベント引数
    /// </summary>
    public sealed class RowAnalyzingEventArgs : EventArgs
    {
        /// <summary>
        /// ログファイル
        /// </summary>
        public LogFile LogFile { get; private set; }
        /// <summary>
        /// 解析する行
        /// </summary>
        public KCVDBRow Row { get; private set; }

        public RowAnalyzingEventArgs(LogFile logFIle, KCVDBRow kcvdbRow)
        {
            this.LogFile = logFIle;
            this.Row = kcvdbRow;
        }
    }
}
