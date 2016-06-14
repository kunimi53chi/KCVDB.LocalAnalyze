using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze
{
    /// <summary>
    /// 艦これサーバー
    /// </summary>
    public class KancolleServer
    {
        /// <summary>
        /// サーバーデータのインデックス
        /// </summary>
        public int Index { get; private set; }
        /// <summary>
        /// サーバーのID
        /// </summary>
        public int ServerId { get; private set; }
        /// <summary>
        /// サーバーの完全名（例：横須賀鎮守府）
        /// </summary>
        public string FullName { get; private set; }
        /// <summary>
        /// サーバーのIPアドレス
        /// </summary>
        public string IPAddress { get; private set; }
        /// <summary>
        /// サーバーデータのバージョン（疎開等でIPが変わったとき用）
        /// </summary>
        public int Version { get; private set; }
        /// <summary>
        /// サーバーの簡易名（例：横須賀）
        /// </summary>
        public string ShortName { get; private set; }
        /// <summary>
        /// サーバー名の区分（例：横須賀鎮守府→鎮守府）
        /// </summary>
        public string Category { get; private set; }
        /// <summary>
        /// サーバーの実装日
        /// </summary>
        public DateTime OpenDate { get; private set; }
        /// <summary>
        /// サーバーの移転日（疎開等）
        /// 移転情報がなければnullを返します
        /// </summary>
        public DateTime? MovedDate { get; private set; }

        internal KancolleServer()
        {
        }

        internal KancolleServer(string rowString)
        {
            var cell = rowString.Split('\t');

            this.Index = int.Parse(cell[0]);
            this.ServerId = int.Parse(cell[1]);
            this.FullName = cell[2];
            this.IPAddress = cell[3];
            this.Version = int.Parse(cell[4]);
            this.ShortName = cell[5];
            this.Category = cell[6];
            this.OpenDate = DateTime.Parse(cell[7]);
            if(cell.Length > 8 && !string.IsNullOrEmpty(cell[8])) this.MovedDate = DateTime.Parse(cell[8]);
        }

        /// <summary>
        /// サーバー不明
        /// </summary>
        public static KancolleServer Unknown
        {
            get
            {
                var result = new KancolleServer();
                result.FullName = "不明";
                result.ShortName = "不明";
                result.Category = "不明";

                return result;
            }
        }
    }
}
