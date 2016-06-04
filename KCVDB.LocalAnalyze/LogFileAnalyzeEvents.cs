using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KCVDB.LocalAnalyze.Event;

namespace KCVDB.LocalAnalyze
{
    public class LogFileAnalyzeEvents
    {
        public EventHandler<LogFileEventArgs> OnFileLoaded { get; set; }
        public EventHandler<RowAnalyzingEventArgs> OnAnalyzing { get; set; }
        public EventHandler<ExceptionEventArgs> OnError { get; set; }
        public EventHandler<LogFileEventArgs> OnCompleted { get; set; }
    }
}
