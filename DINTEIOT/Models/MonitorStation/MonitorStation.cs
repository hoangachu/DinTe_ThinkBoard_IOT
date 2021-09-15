using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;
using DINTEIOT.Models.StationData;
namespace DINTEIOT.Models.MonitorStation
{
    public class MonitorStation
    {
        public int monitorStationID { get; set; }
        public string monitorStationName { get; set; }
        public string monitorStationCode { get; set; }
        public int organID { get; set; }
        public string organName { get; set; }
        public string address { get; set; }     // địa điểm
        public string description { get; set; }
        public float longitude { get; set; }
        public float latitude { get; set; }
        public string siteAddress { get; set; }  // địa danh
        public int totalrecord { get; set; }
        public List<StationData.StationData> ListStationData { get; set; }
        public string ListStationDataJson { get; set; }
        public int type { get; set; }
        public string GuiID { get; set; }
    }
    public class MonitorStationFilter : OptionFilter
    {
        public int organID { get; set; }
    }
  
}
