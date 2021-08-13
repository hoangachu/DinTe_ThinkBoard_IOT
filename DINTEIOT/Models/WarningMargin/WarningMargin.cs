using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace DINTEIOT.Models.WarningMargin
{
    public class WarningMargin
    {
        public int warningMarginID { get; set; }
        public string warningMarginName { get; set; }
        public int warningMarginValueFrom { get; set; }
        public int stationDataID { get; set; }
        public int warningMarginValueTo { get; set; }
        public string warningMarginValueUnit { get; set; }
        public int totalrecord { get; set; }
        public string warningMarginValueColor { get; set; }
        public string stationDataName { get; set; }
    }
    public class WarningMarginFilter : OptionFilter
    {
        public int stationDataID { get; set; } = 0;
    }

}
