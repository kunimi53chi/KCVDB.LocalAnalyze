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
            var log = new Log();
            log.RootPath = @"D:\BlobDataDownloader";
            log.ReadingFile.Subscribe(
                logFile =>
                {
                    Console.WriteLine(logFile.SessionId);
                    logFile.ReadingRow.Subscribe(
                        row =>
                        {
                            rows.Add(row);
                        });
                    rows.Clear();
                });
            log.BeginReading();
        }
    }
}
