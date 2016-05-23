using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var rows = new List<KCVDBRow>();
            new Log(@"D:\BlobDataDownloader\Publish\2016\05\03.7z").Subscribe(
                logFile =>
                {
                    Console.WriteLine(logFile.Path);
                    logFile.Subscribe(
                        line =>
                        {
                            rows.Add(new KCVDBRow(line));
                        });
                    rows.Clear();
                });
        }
    }
}
