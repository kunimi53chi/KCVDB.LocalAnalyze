using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KCVDB.LocalAnalyze
{
    /// <summary>
    /// ログファイルの検索を行うクラス
    /// </summary>
    public static class KCVDBFileSearch
    {
        /// <summary>
        /// ファイルのインデックス用のクラス
        /// </summary>
        public class FileIndex
        {
            /// <summary>
            /// ファイルインデックスの番号の文字列
            /// </summary>
            public string IndexString { get; set; }
            /// <summary>
            /// 日本標準時間でのHttpDateの日付の文字列
            /// </summary>
            public string DateString { get; set; }
            /// <summary>
            /// セッションID
            /// </summary>
            public string SessionId { get; set; }
            /// <summary>
            /// AgentID
            /// </summary>
            public string AgentId { get; set; }

            /// <summary>
            /// ファイル名の相対パス
            /// </summary>
            public string FileName { get; set; }

            /// <summary>
            /// ファイルインデックスのコンストラクタ
            /// </summary>
            /// <param name="index">インデックス番号</param>
            /// <param name="fileName">ファイル名</param>
            /// <param name="rowData"></param>
            public FileIndex(int index, string fileName, KCVDBRow rowData)
            {
                this.IndexString = index.ToString();
                this.DateString = (rowData.HttpDate.ToJst()).Date.ToShortDateString();
                this.SessionId = rowData.SessionId;
                this.AgentId = rowData.AgentId;

                this.FileName = fileName;
            }

            /// <summary>
            /// ファイルインデックスのコンストラクタ
            /// </summary>
            public FileIndex()
            {
            }

            /// <summary>
            /// KCVDBログをファイルインデックスにパースを試みます。戻り値は成功したかどうかを示します
            /// </summary>
            /// <param name="tsvRowString">KCVDBの行単位の文字列</param>
            /// <param name="indexItem"></param>
            /// <returns></returns>
            public static bool TryParse(string tsvRowString, out FileIndex indexItem)
            {
                indexItem = new FileIndex();

                if (tsvRowString == null)
                {
                    return false;
                }
                var cell = tsvRowString.Split('\t');
                if (cell.Length < 5)
                {
                    return false;
                }

                indexItem.IndexString = cell[0];
                indexItem.DateString = cell[1];
                indexItem.SessionId = cell[2];
                indexItem.AgentId = cell[3];
                indexItem.FileName = cell[4];

                return true;
            }

            /// <summary>
            /// TSV形式のテキストに変換します
            /// </summary>
            /// <returns>TSV形式のテキスト</returns>
            public string ToTsvRow()
            {
                return IndexString + "\t" + DateString + "\t" + SessionId + "\t" + AgentId + "\t" + FileName;
            }
        }

        /// <summary>
        /// ファイルインデックスをロード
        /// </summary>
        /// <param name="fileIndexPath"></param>
        /// <returns></returns>
        public static IList<FileIndex> FileIndexLoader(string fileIndexPath)
        {
            var list = new List<FileIndex>();
            foreach(var l in File.ReadAllLines(fileIndexPath))
            {
                KCVDBRow row;
                if(KCVDBRow.TryParse(l, out row))
                {
                    FileIndex index;
                    if(FileIndex.TryParse(l, out index))
                    {
                        list.Add(index);
                    }
                }
            }
            return list;
        }

        private static object _lockObject = new object();
        /// <summary>
        /// ファイルインデックスを作成します。作成後はrootDirectory直下にfileindexファイルが作成されます
        /// </summary>
        /// <param name="rootDirectory">ファイルインデックス</param>
        /// <param name="debugging">コンソール出力をオンにするか（デフォルトではオフ）</param>
        /// <param name="maxParallels">並列実行数。1の場合は並列化されません（デフォルトでは1）</param>
        /// <returns>ファイルインデックスのリスト</returns>
        public static IList<FileIndex> CreateFileIndex(string rootDirectory, bool debugging = false, int maxParallels = 1)
        {
            var list = new List<FileIndex>();
            var files = Directory.GetFiles(rootDirectory);
            int cnt = 1;

            var act = new Action<string>((string f) =>
            {
                if (cnt % 2 == 1 && debugging)
                {
                    Console.WriteLine("ファイルインデックスの作成中… {0} / {1}", cnt, files.Length);
                }
                cnt++;

                var ext = Path.GetExtension(f);
                if (ext != ".gz" && ext != ".log") return;

                //1行目
                var first = KCVDBLogFile.ReadAllLines(f).FirstOrDefault();
                if (first != null)
                {
                    FileIndex index;
                    if (FileIndex.TryParse(first, out index))
                    {
                        lock (_lockObject) list.Add(index);
                    }
                }
            });

            //並列化せずに実行
            if(maxParallels <= 1)
            {
                foreach (var f in files) act(f);
            }
            //並列化して実行
            else
            {
                Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = maxParallels }, f =>
                {
                    act(f);
                });
            }

            //ファイルに保存
            try
            {
                File.WriteAllLines(rootDirectory + "\\fileindex", list.Select(x => x.ToTsvRow()));
            }
            catch(Exception ex)
            {
                Console.WriteLine("KCVDB.LocalAnalyze : FileIndexの作成に失敗しました");
                Console.WriteLine(ex.ToString());
            }

            return list;
        }

        /// <summary>
        /// ファイルインデックスからAgentIDを条件としてファイルを抜き出すメソッド
        /// デフォルトでは部分一致ですが、isExtractMatch = trueにすることで完全一致で検索します
        /// </summary>
        /// <param name="fileIndex">ファイルインデックスのコレクション</param>
        /// <param name="matchConditionStr">AgentIDの抽出条件</param>
        /// <param name="isExactMatch">完全一致で検索するかどうか</param>
        /// <returns>抽出されたファイルインデックス</returns>
        public static IEnumerable<FileIndex> AgentIdMatches(this IEnumerable<FileIndex> fileIndex, string matchConditionStr, bool isExactMatch = false)
        {
            foreach(var f in fileIndex)
            {
                //完全一致の場合
                if(isExactMatch)
                {
                    if (f.AgentId == matchConditionStr) yield return f;
                }
                else
                {
                    if (f.AgentId.Contains(matchConditionStr)) yield return f;
                }
            }
        }

    }
}
