using System;

namespace KCVDB.LocalAnalyze
{
    public abstract class LogFile : IObservable<string>
    {
        protected LogFile(Log log)
        {
            this.Log = log;
        }

        public readonly Log Log;

        public abstract string Path { get; }

        public Guid SessionId
        {
            get
            {
                if (sessionId == null)
                {
                    sessionId = new Guid(System.IO.Path.GetFileNameWithoutExtension(this.Path));
                }
                return sessionId.Value;
            }
        }

        public abstract IDisposable Subscribe(IObserver<string> observer);

        private Guid? sessionId;
    }
}
