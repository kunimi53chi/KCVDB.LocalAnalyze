using System.ComponentModel;

namespace KCVDB.LocalAnalyze
{
    public sealed class ReadingLogFileEventArgs : CancelEventArgs
    {
        internal ReadingLogFileEventArgs(LogFile logFile)
        {
            this.LogFile = logFile;
        }

        public readonly LogFile LogFile;
    }
}
