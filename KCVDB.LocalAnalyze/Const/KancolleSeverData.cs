using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.Const
{
    /// <summary>
    /// 艦これサーバーのデータ
    /// </summary>
    public static class KancolleSeverData
    {
        /// <summary>
        /// 艦これサーバーのリスト
        /// </summary>
        public static IList<KancolleServer> ServerList { get; private set; }

        static KancolleSeverData()
        {
            ServerList = new List<KancolleServer>();

            var row = Properties.Resources.KCServerList.Replace("\r\n", "\n").Split('\n').Skip(1);
            foreach (var r in row) ServerList.Add(new KancolleServer(r));
        }
    }
}
