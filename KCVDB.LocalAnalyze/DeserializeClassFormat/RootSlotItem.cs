using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.DeserializeClassFormat
{
    public class RootSlotItem
    {
        public List<ApiSlotitem> api_data;

        public class ApiSlotitem
        {
            public int api_id { get; set; }
            public int api_slotitem_id { get; set; }
            public int api_level { get; set; }
            public int api_alv { get; set; }
        }
    }
}
