using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace DINTEIOT.Models.StationMethod
{
    public class StationMethod
    {
        public int stationMethodID { get; set; }
        public string stationMethodName { get; set; }
        public string stationMethodCode { get; set; }
        public int stationDataID { get; set; }
        public string stationDataName { get; set; }
        public string description { get; set; }
        public int totalrecord { get; set; }
    }
    public class StationMethodFilter : OptionFilter
    {
        public int stationDataID { get; set; }
    }
}
