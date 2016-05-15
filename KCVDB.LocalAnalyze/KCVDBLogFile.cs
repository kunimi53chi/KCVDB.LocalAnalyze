using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace KCVDB.LocalAnalyze
{
    /// <summary>
    /// KCVDBのログファイルIOのクラス
    /// </summary>
    public static class KCVDBLogFile
    {
        /// <summary>
        /// GZIPファイルを解凍し、ファイルの全ての行を読み込みます
        /// </summary>
        /// <param name="filePath">ファイルのパス</param>
        /// <returns>全ての行の文字列。読み込みに失敗した場合はnullを返します</returns>
        public static string DecompressGzipAndReadAllText(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException("KCVDB.LocalAnalyze : 解凍ファイルが存在しません");
            if (Path.GetExtension(filePath) != ".gz") throw new ArgumentException("KCVDB.LocalAnalyze : 解凍ファイルがgzipではありません");

            var ms = new MemoryStream();
            var inStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var decompStream = new GZipStream(inStream, CompressionMode.Decompress);
            var sr = new StreamReader(ms);
            string text = null;
            try
            {
                int num;
                var buf = new byte[4096];
                //解凍
                while ((num = decompStream.Read(buf, 0, buf.Length)) > 0)
                    ms.Write(buf, 0, num);
                //MemoryStreamの読み込み
                ms.Position = 0;
                text = sr.ReadToEnd();
            }
            catch(Exception ex)
            {
                Console.WriteLine("KCVDB.LocalAnalyze : gzipファイルの解凍に失敗しました");
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                ms.Dispose();
                inStream.Dispose();
                decompStream.Dispose();
                sr.Dispose();
            }

            return text;
        }

        /// <summary>
        /// GZIPファイルを解凍し、ファイルの全ての行を読み込み行ごとに分割します
        /// </summary>
        /// <param name="filePath">ファイルのパス</param>
        /// <returns>行単位の文字列。読み込みに失敗した場合は空の配列を返します</returns>
        public static string[] DecompressGzipAndReadAllLines(string filePath)
        {
            var text = DecompressGzipAndReadAllText(filePath);
            if (text != null) return text.Replace(Environment.NewLine, "\n").Split('\n');
            else return new string[0];
        }

        /// <summary>
        /// ファイル形式ごとに必要に応じて解凍しながら、全ての行を読み込みます（.gz, .logのみ対応）
        /// </summary>
        /// <param name="filePath">ファイルのパス</param>
        /// <returns>全ての行の文字列。読み込みに失敗した場合はnullを返します</returns>
        public static string ReadAllText(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException("KCVDB.LocalAnalyze : 読込ファイルが存在しません");

            string text = null;
            
            switch(Path.GetExtension(filePath))
            {
                case ".log":
                    try
                    {
                        text = File.ReadAllText(filePath);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("KCVDB.LocalAnalyze : ログファイルの読込に失敗しました");
                        Console.WriteLine(ex.ToString());
                    }
                    break;
                case ".gz":
                    text = DecompressGzipAndReadAllText(filePath);
                    break;
                default:
                    throw new FormatException("KCVDB.LocalAnalyze : 読込ファイル形式が正しくありません");
            }

            return text;
        }

        /// <summary>
        /// ファイル形式ごとに必要に応じて解凍しながら読み込み、行ごとに分割します
        /// </summary>
        /// <param name="filePath">ファイルのパス</param>
        /// <returns>行単位の文字列。読み込みに失敗した場合は空の配列を返します</returns>
        public static string[] ReadAllLines(string filePath)
        {
            var text = ReadAllText(filePath);
            if (text != null) return text.Replace(Environment.NewLine, "\n").Split('\n');
            else return new string[0];            
        }

        /// <summary>
        /// ファイル形式ごとに必要に応じて解凍しながら読み込み、行単位でパースします
        /// </summary>
        /// <param name="filePath">ファイルのパス</param>
        /// <returns>行単位のパースされたアイテム</returns>
        public static IEnumerable<KCVDBRow> ParseAllLines(string filePath)
        {
            foreach(var l in ReadAllLines(filePath))
            {
                KCVDBRow row;
                if (!KCVDBRow.TryParse(l, out row)) yield break;
                else yield return row;
            }
        }
    }
}
