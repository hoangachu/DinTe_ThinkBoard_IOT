using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;

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
        public int longitude { get; set; }
        public int latitude { get; set; }
        public string siteAddress { get; set; }  // địa danh
        public int totalrecord { get; set; }
    }
    public class MonitorStationFilter : OptionFilter
    {
        public int organID { get; set; }
    }
}
