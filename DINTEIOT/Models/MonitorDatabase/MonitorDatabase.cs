using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace DINTEIOT.Models.MonitorDatabase
{
    public class MonitorDatabase
    {
        public int monitorDatabaseID { get; set; }
        public string monitorDatabaseTime { get; set; }
        public int monitorDatabaseValue { get; set; }
        public string monitorDatabaseUnit { get; set; }
        public int stationDataID { get; set; }
        public string stationDataName { get; set; }
        public int monitorStationID { get; set; }
        public int totalrecord { get; set; }
    }
    public class MonitorDatabaseFilter : OptionFilter
    {
        public int monitorID { get; set; }
        public int stationDataID { get; set; }
    }
}
