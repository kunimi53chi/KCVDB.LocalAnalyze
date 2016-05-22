using System.ComponentModel;

namespace KCVDB.LocalAnalyze
{
    public sealed class ReadingLogLineEventArgs : CancelEventArgs
    {
        internal ReadingLogLineEventArgs(string line)
        {
            this.Line = line;
        }

        public readonly string Line;
    }
}
