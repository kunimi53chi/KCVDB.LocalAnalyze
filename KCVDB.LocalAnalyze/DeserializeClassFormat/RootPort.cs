using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.DeserializeClassFormat
{
    public class RootPort
    {
        public ApiPort api_data { get; set; }

        public class ApiPort
        {
            public ApiBasic api_basic { get; set; }
        }

        public class ApiBasic
        {
            /// <summary>
            /// 提督ID
            /// </summary>
            public int api_member_id { get; set; }
            /// <summary>
            /// 出撃勝利数
            /// </summary>
            public int api_st_win { get; set; }
            /// <summary>
            /// 出撃敗北数
            /// </summary>
            public int api_st_lose { get; set; }
            /// <summary>
            /// 遠征数
            /// </summary>
            public int api_ms_count { get; set; }
            /// <summary>
            /// 遠征成功数
            /// </summary>
            public int api_ms_success { get; set; }
            /// <summary>
            /// 演習勝利数
            /// </summary>
            public int api_pt_win { get; set; }
            /// <summary>
            /// 演習敗北数
            /// </summary>
            public int api_pt_lose { get; set; }
        }
    }
}
