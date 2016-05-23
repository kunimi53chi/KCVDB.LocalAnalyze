using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.IO
{
    internal static class ObservableTextReaderExtensions
    {
        public static IObservable<string> ReadLine(this IObservable<ArraySegment<byte>> source)
        {
            var subject = new ObservableTextReader();
            source.Subscribe(subject);
            return subject;
        }
    }
}
