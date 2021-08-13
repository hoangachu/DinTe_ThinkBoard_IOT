using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace DINTEIOT.Models.StationDriver
{
    public class StationDriver
    {
        public int stationDriverID { get; set; }
        public string stationDriverName { get; set; }
        public string stationDriverCode { get; set; }
        public string stationDriverType { get; set; }
        public int monitorStationID { get; set; }
        public string monitorStationName { get; set; }
        public string description { get; set; }
        public int totalrecord { get; set; }
    }
    public class StationDriverFilter : OptionFilter
    {
        public int monitorStationID { get; set; }
    }
}
