using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.DeserializeClassFormat
{
    public class RootStart2
    {
        public RootData api_data { get; set; }

        public class RootData
        {
            public List<ApiMstShip> api_mst_ship { get; set; }
            public List<ApiMstStype> api_mst_stype { get; set; }
            public List<ApiMstSlotitem> api_mst_slotitem { get; set; }
        }

        public class ApiMstShip
        {
            public int api_id { get; set; }
            public int api_stype { get; set; }
            public int api_bull_max { get; set; }
        }

        public class ApiMstStype
        {
            public int api_id { get; set; }
            public string api_name { get; set; }
        }

        public class ApiMstSlotitem
        {
            public int api_id { get; set; }
            public string api_name { get; set; }
        }
    }
}
