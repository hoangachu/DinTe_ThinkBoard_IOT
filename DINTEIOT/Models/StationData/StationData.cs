using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace DINTEIOT.Models.StationData
{
    public class StationData
    {
        public int stationDataId { get; set; }
        public string  stationDataName { get; set; }
        public string stationDataCode { get; set; }
        public long totalrecord { get; set; }
    }
    public class StationDataFilter : OptionFilter
    {

    }
}


