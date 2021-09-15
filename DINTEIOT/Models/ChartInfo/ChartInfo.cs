using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace DINTEIOT.Models.ChartInfo
{
    public class ChartInfo
    {
        public List<int> listvalue { get; set; }
        public List<double> listvaluedouble { get; set; }
        public List<TimeSeriesValue> listTimeSeriesValue { get; set; }
        public List<DINTEIOT.Models.WarningMargin.WarningMargin> listWarningMargin { get; set; }
        public string chartName { get; set; }
        public string stationDataName { get; set; }
        public string monitorStationName { get; set; }
        public int monitorStationID { get; set; }
        public string monitorStationstringId { get; set; }
        public int stationDataID { get; set; }
        public string stationDatakey { get; set; }
        public DateTime dateTime { get; set; }
    }
    public class ChartInfoFilter : OptionFilter
    {
        public int monitorStationID { get; set; }
        public string GuiID { get; set; }
        public int stationDataID { get; set; }
        public int dateTimeType { get; set; }
        public string[] liststationdatakey { get; set; }
    }
    public class TimeSeriesValue
    {
        public double ts { get; set; }
        public string value { get; set; }
        public double valuedouble { get; set; }
        public DateTime time { get; set; }
        public int rownumber { get; set; }
    }
    public class ListTimeSeriesValue
    {
        public List<TimeSeriesValue> list { get; set; }
    }
}
