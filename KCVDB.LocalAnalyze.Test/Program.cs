using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace KCVDB.LocalAnalyze.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();


            var events = new LogFileAnalyzeEvents();
            var filecount = 0;
            var line = 0;
            events.OnFileLoaded += (_, e) =>
                {
                    Console.Write("{0}:", e.LogFile.Path);
                    filecount++;
                    Console.Title = filecount.ToString();
                    line = 0;
                };
            events.OnAnalyzing += (_, e) =>
                {
                    line++;
                };
            events.OnError += (_, e) =>
                {
                    Console.WriteLine(e.Exception.ToString());
                };
            events.OnCompleted += (_, e) =>
                {
                    Console.WriteLine("{0}行", line);
                };

            KCVDBLogFile.AnalyzeAllSevenZipArchives(@"D:\blob\2016-05\2016-05-09.7z", events, 1000, 50);

            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }
    }
}
