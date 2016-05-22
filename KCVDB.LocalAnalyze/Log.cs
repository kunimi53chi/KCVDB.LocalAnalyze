using System;
using System.IO;
using System.IO.Compression;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text.RegularExpressions;
using KCVDB.LocalAnalyze.IO;
using SevenZip;

namespace KCVDB.LocalAnalyze
{
    public sealed class Log
    {
        public string RootPath;

        public DateTimeOffset StartDateTime;

        public DateTimeOffset EndDateTime;

        private readonly Subject<LogFile> readingFile;

        public readonly IObservable<LogFile> ReadingFile;

        public Log()
        {
            this.StartDateTime = DateTimeOffset.MinValue;
            this.EndDateTime = DateTimeOffset.MaxValue;
            this.readingFile = new Subject<LogFile>();
            this.ReadingFile = this.readingFile.AsObservable();
        }

        public void BeginReading()
        {
            var rootInfo = new DirectoryInfo(System.IO.Path.Combine(Assembly.GetExecutingAssembly().Location, this.RootPath));
            foreach (var yearInfo in rootInfo.EnumerateDirectories())
            {
                int year;
                if (int.TryParse(yearInfo.Name, out year))
                {
                    foreach (var monthInfo in yearInfo.EnumerateDirectories())
                    {
                        int month;
                        if (int.TryParse(monthInfo.Name, out month))
                        {
                            foreach (var dayInfo in monthInfo.EnumerateDirectories())
                            {
                                int day;
                                if (int.TryParse(dayInfo.Name, out day))
                                {
                                    this.StartReading(ToDate(year, month, day), dayInfo);
                                }
                            }
                            foreach (var dayInfo in monthInfo.EnumerateFiles())
                            {
                                int day;
                                if (int.TryParse(dayInfo.Name, out day))
                                {
                                    this.StartReading(ToDate(year, month, day), dayInfo);
                                }
                            }
                        }
                    }
                }
            }
            foreach (var fileInfo in rootInfo.EnumerateFiles())
            {
                var array = fileInfo.Name.Split(new string[] { "-" }, StringSplitOptions.None);
                int year;
                int month;
                int day;
                if (array.Length == 3 && int.TryParse(array[0], out year) && int.TryParse(array[1], out month) && int.TryParse(array[2], out day))
                {
                    this.StartReading(ToDate(year, month, day), fileInfo);
                }
            }
            this.readingFile.OnCompleted();
        }

        private static DateTimeOffset ToDate(int year, int month, int day)
        {
            return new DateTimeOffset(year, month, day, 0, 0, 0, new TimeSpan(9, 0, 0));
        }

        private void StartReading(DateTimeOffset date, FileInfo fileInfo)
        {
            using (var extractor = new SevenZipExtractor(fileInfo.FullName))
            {
                extractor.ExtractFiles(args =>
                {
                    var logFile = new SevenZipLogFile(this, date, GetSessionId(fileInfo.Name));
                    this.readingFile.OnNext(logFile);
                    logFile.StartReading(args);
                });
            }
        }

        private void StartReading(DateTimeOffset date, DirectoryInfo directoryInfo)
        {
            foreach (var fileInfo in directoryInfo.EnumerateFiles())
            {
                Stream stream = fileInfo.OpenRead();
                if (fileInfo.Extension == ".gz")
                {
                    stream = new GZipStream(stream, System.IO.Compression.CompressionMode.Decompress);
                }
                using (stream)
                {
                    var logFile = new StreamLogFile(this, date, GetSessionId(fileInfo.Name));
                    this.readingFile.OnNext(logFile);
                    logFile.StartReading(stream);
                }
            }
        }

        private static Guid GetSessionId(string name)
        {
            return new Guid(System.IO.Path.GetFileNameWithoutExtension(name));
        }

        static Log()
        {
            if (IntPtr.Size == 4)
            {
                SevenZipBase.SetLibraryPath(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "x86", "7z.dll"));
            }
            else if (IntPtr.Size == 8)
            {
                SevenZipBase.SetLibraryPath(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "x64", "7z.dll"));
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        //private static readonly Regex yearRegex = new Regex("^(?<year>[0-9]{4})$", RegexOptions.Compiled);
        //private static readonly Regex yearMonthDayRegex = new Regex("^(?<year>[0-9]{4})-(?<month>[0-9]{2})-(?<day>[0-9]{2})$", RegexOptions.Compiled);
        //private static readonly Regex monthRegex = new Regex("^(?<month>[0-9]{2})$", RegexOptions.Compiled);
        //private static readonly Regex monthDayRegex = new Regex("^(?<month>[0-9]{2})-(?<day>[0-9]{2})", RegexOptions.Compiled);
        //private static readonly Regex dayRegex = new Regex("^(?<day>[0-9]{2})$", RegexOptions.Compiled);
    }
}
