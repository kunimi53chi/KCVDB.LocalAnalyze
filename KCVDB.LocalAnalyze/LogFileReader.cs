//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.IO;
//using System.Globalization;
//using System.IO.Compression;
//using KCVDB.LocalAnalyze.IO;
//using System.Text.RegularExpressions;
//using SevenZip;

//namespace KCVDB.LocalAnalyze
//{
//    public static class LogFileReader
//    {
//        //public static IEnumerable<LogSession> ReadAllSessions(string directoryPath, DateTimeOffset startDateTime, DateTimeOffset endDateTime)
//        //{
//        //    var days = ReadDayPathByDate(directoryPath)
//        //        .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2)
//        //        .OrderBy(pair => pair.Key)
//        //        .SkipWhile(pair => pair.Key.AddDays(1) < startDateTime)
//        //        .TakeWhile(pair => pair.Key < endDateTime)
//        //        .Select(pair => new Day { Date = pair.Key, Path = pair.Value })
//        //        .ToArray();
//        //    for (var i = 0; i < days.Length; ++i)
//        //    {
//        //        var day = days[i];
//        //        if (day.Entry == null)
//        //        {
//        //            day.Entry = DayEntry.Create(day.Date, day.Path);
//        //            day.SessionIds = new HashSet<string>(day.Entry.SessionIds);
//        //        }
//        //        foreach (var tuple in day.Entry.OpenStreams(day.SessionIds))
//        //        {
//        //            var sessionId = tuple.Item1;
//        //            var stream = tuple.Item2;
//        //            var logs = new List<Tuple<DateTimeOffset, Stream>> { Tuple.Create(day.Date, stream) };
//        //            for (var j = i + 1; j < days.Length; ++j)
//        //            {
//        //                var nextDay = days[j];
//        //                if (nextDay.Entry == null)
//        //                {
//        //                    nextDay.Entry = DayEntry.Create(nextDay.Date, nextDay.Path);
//        //                    nextDay.SessionIds = new HashSet<string>(nextDay.Entry.SessionIds);
//        //                }
//        //                if (nextDay.SessionIds.Contains(sessionId))
//        //                {
//        //                    logs.Add(Tuple.Create(nextDay.Date, nextDay.Entry.OpenStream(sessionId)));
//        //                    nextDay.SessionIds.Remove(sessionId);
//        //                }
//        //                else
//        //                {
//        //                    break;
//        //                }
//        //            }
//        //            yield return new LogSession(sessionId, logs);
//        //        }
//        //    }
//        //}

//        //private static IEnumerable<LogSession> ReadAllSessions(Day[] dayInfos, int todayIndex, IEnumerable<string> todaySessionIds)
//        //{
//        //    var todayInfo = dayInfos[todayIndex];
//        //    todayInfo.Entry = DayEntry.Create(todayInfo.Date, todayInfo.Path);
//        //    todayInfo.SessionIds = new HashSet<string>(todayInfo.Entry.SessionIds);
//        //    for (var tomorrowIndex = todayIndex + 1; tomorrowIndex < dayInfos.Length; ++tomorrowIndex)
//        //    {
//        //        var tomorrowInfo = dayInfos[tomorrowIndex];
//        //        if (tomorrowInfo.Entry == null)
//        //        {
//        //            tomorrowInfo.Entry = DayEntry.Create(tomorrowInfo.Date, tomorrowInfo.Path);
//        //            tomorrowInfo.SessionIds = new HashSet<string>(tomorrowInfo.Entry.SessionIds);
//        //        }
//        //        var sessionIds = new List<string>();
//        //        var tomorrowSessionIds = new List<string>();
//        //        foreach (var sessionId in todaySessionIds)
//        //        {
//        //            if (tomorrowInfo.SessionIds.Contains(sessionId))
//        //            {
//        //                tomorrowSessionIds.Add(sessionId);
//        //            }
//        //            else
//        //            {
//        //                sessionIds.Add(sessionId);
//        //            }
//        //        }
//        //        foreach (var stream in todayInfo.Entry.OpenStreams(sessionIds))
//        //        {
//        //            yield return new LogSession(stream;
//        //        }
//        //    }
//        //}

//        private static IEnumerable<Tuple<DateTimeOffset, string>> ReadDayPaths(string directoryPath)
//        {
//            var offset = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time").BaseUtcOffset;
//            var rootDirectoryInfo = new DirectoryInfo(directoryPath);
//            //foreach (var yearFileInfo in rootDirectoryInfo.EnumerateFiles())
//            //{
//            //    yield return Tuple.Create($"{Path.GetFileNameWithoutExtension(yearFileInfo.Name)}", yearFileInfo.FullName);
//            //}
//            foreach (var yearDirectoryInfo in rootDirectoryInfo.EnumerateDirectories())
//            {
//                int year;
//                if (int.TryParse(yearDirectoryInfo.Name, out year))
//                {
//                    //foreach (var monthFileInfo in yearDirectoryInfo.EnumerateFiles())
//                    //{
//                    //    yield return Tuple.Create($"{yearDirectoryInfo.Name}-{Path.GetFileNameWithoutExtension(monthFileInfo.Name)}", monthFileInfo.FullName);
//                    //}
//                    foreach (var monthDirectoryInfo in yearDirectoryInfo.EnumerateDirectories())
//                    {
//                        int month;
//                        if (int.TryParse(monthDirectoryInfo.Name, out month))
//                        {
//                            foreach (var dayFileInfo in monthDirectoryInfo.EnumerateFiles())
//                            {
//                                int day;
//                                if (int.TryParse(dayFileInfo.Name, out day))
//                                {
//                                    yield return Tuple.Create(new DateTimeOffset(year, month, day, 0, 0, 0, offset), dayFileInfo.FullName);
//                                }
//                            }
//                            foreach (var dayDirectoryInfo in monthDirectoryInfo.EnumerateDirectories())
//                            {
//                                int day;
//                                if (int.TryParse(dayDirectoryInfo.Name, out day))
//                                {
//                                    yield return Tuple.Create(new DateTimeOffset(year, month, day, 0, 0, 0, offset), dayDirectoryInfo.FullName);
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        private static IEnumerable<string> ReadLogPaths(string datePath)
//        {
//            var directoryInfo = new DirectoryInfo(datePath);
//            if (directoryInfo.Exists)
//            {
//                foreach ()
//            }
//            else
//            {
//                var dateFileInfo = new FileInfo(datePath);
//                if (dateFileInfo.Exists)
//                {
//                }
//                else
//                {
//                    throw new FileNotFoundException();
//                }
//            }
//        }

//        private static IEnumerable<KeyValuePair<string, Stream>> OpenAllStreams(string fullPath)
//        {
//            var directoryInfo = new DirectoryInfo(fullPath);
//            if (directoryInfo.Exists)
//            {
//                return directoryInfo.EnumerateFiles().SelectMany(fileInfo => OpenAllStreams(fileInfo));
//            }
//            else
//            {
//                var fileInfo = new FileInfo(fullPath);
//                if (fileInfo.Exists)
//                {
//                    return OpenAllStreams(fileInfo);
//                }
//                else
//                {
//                    throw new FileNotFoundException();
//                }
//            }
//        }

//        private static IEnumerable<KeyValuePair<string, Stream>> OpenAllStreams(FileInfo fileInfo)
//        {
//            if (fileInfo.Extension == ".log")
//            {
//                return new[] { new KeyValuePair<string, Stream>(fileInfo.Name, fileInfo.OpenRead()) };
//            }
//            else if (fileInfo.Extension == ".gz")
//            {
//                return new[] { new KeyValuePair<string, Stream>(fileInfo.Name, new GZipStream(fileInfo.OpenRead(), CompressionMode.Decompress)) };
//            }
//            else
//            {
//                return Archiver.ExtractToStreams(fileInfo);
//            }
//        }

//        //private static readonly Regex yearRegex = new Regex("^(?<year>[0-9]{4})$", RegexOptions.Compiled);
//        //private static readonly Regex yearMonthDayRegex = new Regex("^(?<year>[0-9]{4})-(?<month>[0-9]{2})-(?<day>[0-9]{2})$", RegexOptions.Compiled);
//        //private static readonly Regex monthRegex = new Regex("^(?<month>[0-9]{2})$", RegexOptions.Compiled);
//        //private static readonly Regex monthDayRegex = new Regex("^(?<month>[0-9]{2})-(?<day>[0-9]{2})", RegexOptions.Compiled);
//        //private static readonly Regex dayRegex = new Regex("^(?<day>[0-9]{2})$", RegexOptions.Compiled);

//        //private sealed class Day
//        //{
//        //    public DateTimeOffset Date;
//        //    public string Path;
//        //    public DayEntry Entry;
//        //    public HashSet<string> SessionIds;
//        //}
//    }
//}
