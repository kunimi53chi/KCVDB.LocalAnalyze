using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KCVDB.LocalAnalyze.Event;

namespace KCVDB.LocalAnalyze
{
    /// <summary>
    /// ログファイルを解析する際の処理イベント（Rxラップ用）
    /// </summary>
    public class LogFileAnalyzeEvents
    {
        /// <summary>
        /// ファイルが展開されたときに呼ばれるイベント（1番目）
        /// </summary>
        public EventHandler<LogFileEventArgs> OnFileLoaded { get; set; }
        /// <summary>
        /// 行単位での解析を行ったときに呼ばれるイベント（2番目）
        /// </summary>
        public EventHandler<RowAnalyzingEventArgs> OnAnalyzing { get; set; }
        /// <summary>
        /// OnAnalyzingで例外が吐かれた際に呼ばれるイベント（3番目）
        /// </summary>
        public EventHandler<ExceptionEventArgs> OnError { get; set; }
        /// <summary>
        /// 全ての行の処理が終わった際に呼ばれるイベント（4番目）
        /// </summary>
        public EventHandler<LogFileEventArgs> OnCompleted { get; set; }
    }
}
