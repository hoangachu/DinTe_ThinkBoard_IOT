using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DINTEIOT.Helpers;
using DINTEIOT.Helpers.Common;
using DINTEIOT.Models.ChartInfo;
using DINTEIOT.Models.StationData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Helpers;

namespace DINTEIOT.Controllers
{
    public class ChartInfoController : Controller
    {
        private IMonitorStationController _imonitorStationController;
        private IStationDataController _iStationDataController;
        private IMonitorDatabaseController _iMonitorDatabaseController;
        private IWarningMarginController _iWarningMarginController;
        private string responseContent;
        public ChartInfoController(IMonitorStationController IMonitorStationController, IStationDataController IStationDataController, IMonitorDatabaseController IMonitorDatabaseController,
            IWarningMarginController IWarningMarginController)
        {
            _imonitorStationController = IMonitorStationController;
            _iStationDataController = IStationDataController;
            _iMonitorDatabaseController = IMonitorDatabaseController;
            _iWarningMarginController = IWarningMarginController;
        }
        public IActionResult Index()
        {
            ChartInfoFilter chartInfoFilter = new ChartInfoFilter();
            ViewBag.TramQuanTrac = _imonitorStationController.GetAllListMonitorStation();
            ViewBag.Liststationdata = null;
            if (TempData["ChartInfoFilter"] != null) { chartInfoFilter = JsonConvert.DeserializeObject<ChartInfoFilter>((string)TempData["ChartInfoFilter"]); TempData.Keep();
                ViewBag.Liststationdata = JsonConvert.SerializeObject(chartInfoFilter.liststationdatakey); }
            ViewBag.CurrentPageName = ScreenName.ChartInfo;
            return View();
        }
        public IActionResult MultiStation()
        {
            ViewBag.TramQuanTrac = _imonitorStationController.GetAllListMonitorStation();
            ViewBag.LoaiDuLieu = _iStationDataController.GetAllListStationData();
            return View();
        }

        public async Task<IActionResult> GetTimesSeriesKey(string id) //GetTimesSeriesKey 
        {
            using var httpClient = new HttpClient();
            LoginHelper.LoginHelper.GetThinkBoardToken();
            //set header
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml+json");
            httpClient.DefaultRequestHeaders.Add("X-Authorization", "bearer " + Startup.thinkportaccesstoken);
            try
            {
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                httpRequestMessage.RequestUri = new Uri(Startup.ConnectionStringsThinkBoard + "/api/plugins/telemetry/" + "DEVICE" + "/" + id + "/keys/timeseries");
                var response = await httpClient.SendAsync(httpRequestMessage);
                responseContent = await response.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
            }
            return Ok(new { data = responseContent });
        }
        public async Task<IActionResult> GetLastTimeSeries(string id) //GetLastTimeSeries - lấy dl mới nhất
        {
            using var httpClient = new HttpClient();
            LoginHelper.LoginHelper.GetThinkBoardToken();
            //set header
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml+json");
            httpClient.DefaultRequestHeaders.Add("X-Authorization", "bearer " + Startup.thinkportaccesstoken);
            try
            {
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                httpRequestMessage.RequestUri = new Uri(Startup.ConnectionStringsThinkBoard + "/api/plugins/telemetry/DEVICE/"+ id + "/values/timeseries?useStrictDataTypes=false");
                var response = await httpClient.SendAsync(httpRequestMessage);
                responseContent = await response.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
            }
            return Ok(new { data = responseContent });
        }

        public IActionResult GetMonitorStation()
        {
            var data = _imonitorStationController.GetAllListMonitorStation();
            return Ok(new { data = data });
        }
        [HttpGet]
        public IActionResult GetListChart(int monitorStationID = 0, int stationDataID = 0, int type = 0, string key = null)
        {
            List<ChartInfo> listChartInfos = new List<ChartInfo>();

            if (stationDataID > 0)
            {
                listChartInfos.Add(GetListChartByMonitorStationAndStationData(monitorStationID, stationDataID));
            }
            else
            {
                listChartInfos = GetListChartByMonitorStationID(monitorStationID);
            }
            return Ok(new { data = listChartInfos });
        }
        public async Task<IActionResult> GetDataByStationIDandKeys(string id, string key, DateTime? startDate, DateTime? endDate, int typedatetime) //getTimeseries - lấy ds dữ liệu theo trạm,loại dl ,thời gian
        {
            //List<ChartInfo> listChartInfo = new List<ChartInfo>();
            ChartInfo chartInfo = new ChartInfo();
            chartInfo.listTimeSeriesValue = new List<TimeSeriesValue>();
            chartInfo.monitorStationstringId = id;
            chartInfo.stationDatakey = key;
            chartInfo.chartName = "Biểu đồ " + key;
            chartInfo.stationDataName = key;
            try
            {
                DateTime today = DateTime.Now /* new DateTime(2021, 9, 1, 0, 0, 0)*/;
                if (typedatetime == 1)
                {
                    for (var item = 1; item <= 7; item++)
                    {
                        var datetime = today.AddDays(item * (-1));
                        ChartInfo chartInfonew = GetChartInfoByMoniterStationIDandStationDataIDFromApi(id, key, datetime, datetime).Result;
                        TimeSeriesValue timeSeriesValue = new TimeSeriesValue();
                        timeSeriesValue.time = datetime;
                        timeSeriesValue.valuedouble = chartInfonew.listvaluedouble.Average();
                        chartInfo.listTimeSeriesValue.Add(timeSeriesValue);
                    }
                }
                else if (typedatetime == 2)
                {
                    for (var item = 1; item <= 30; item++)
                    {
                        var datetime = today.AddDays(item * (-1));
                        ChartInfo chartInfonew = GetChartInfoByMoniterStationIDandStationDataIDFromApi(id, key, datetime, datetime).Result;
                        TimeSeriesValue timeSeriesValue = new TimeSeriesValue();
                        timeSeriesValue.time = datetime;
                        timeSeriesValue.valuedouble = chartInfonew.listvaluedouble.Average();
                        chartInfo.listTimeSeriesValue.Add(timeSeriesValue);
                    }
                }
                else
                {
                    var month = today.Month;
                    for (var item = 0; item <= 2; item++)
                    {
                        var datetime = today.AddMonths(item * (-1));
                        var startDatev1 = new DateTime(datetime.Year, datetime.Month, 1);
                        var endDatev1 = startDatev1.AddMonths(1).AddDays(-1);
                        List<double> listvaluebydayarr = new List<double>();
                        for (var itemv1 = startDatev1.Day; itemv1 <= endDatev1.Day; itemv1++)
                        {
                            startDate = new DateTime(datetime.Year, datetime.Month, itemv1);
                            endDate = new DateTime(datetime.Year, datetime.Month, itemv1);
                            ChartInfo chartInfonew = GetChartInfoByMoniterStationIDandStationDataIDFromApi(id, key, startDate, endDate).Result;
                            listvaluebydayarr.Add(chartInfonew.listvaluedouble.Average());
                           
                        }
                        TimeSeriesValue timeSeriesValue = new TimeSeriesValue();
                        timeSeriesValue.time = datetime;
                        timeSeriesValue.valuedouble = listvaluebydayarr.Average();
                        chartInfo.listTimeSeriesValue.Add(timeSeriesValue);
                        //ChartInfo chartInfonew = GetChartInfoByMoniterStationIDandStationDataIDFromApi(id, key, datetime, datetime).Result;
                        //TimeSeriesValue timeSeriesValue = new TimeSeriesValue();
                        //timeSeriesValue.time = datetime;
                        //timeSeriesValue.valuedouble = chartInfonew.listvaluedouble.Average();
                        //chartInfo.listTimeSeriesValue.Add(timeSeriesValue);
                    }
                   
                }

                //listChartInfo.Add(chartInfo);
            }
            catch (Exception ex)
            {
            }
            return Ok(new { data = chartInfo });
        }
        public async Task<IActionResult> GetDataByStationIDandKeysAndDate(string id, string key, DateTime? startDate, DateTime? endDate,int pagenumber = 1) // 
        {
            using var httpClient = new HttpClient();
            LoginHelper.LoginHelper.GetThinkBoardToken();

            List<TimeSeriesValue> listTimeSeriesValue = new List<TimeSeriesValue>();
            //set header
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml+json");
            httpClient.DefaultRequestHeaders.Add("X-Authorization", "bearer " + Startup.thinkportaccesstoken);
            try
            {

                startDate = startDate == null ? null : DateTime.Now;
                endDate = endDate == null ? null : DateTime.Now;
                var epochfirst = (new DateTime(DateTime.Parse(startDate.Value.ToString()).Year, 8 /*DateTime.Parse(startDate.Value.ToString()).Month*/, 26, 00, 00, 0, 0, System.DateTimeKind.Utc) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds * 1000;
                var epochlast = (new DateTime(DateTime.Parse(endDate.Value.ToString()).Year, 8/*DateTime.Parse(endDate.Value.ToString()).Month*/, 26, 23, 45, 0, 0, System.DateTimeKind.Utc) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds * 1000;
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                httpRequestMessage.RequestUri = new Uri(Startup.ConnectionStringsThinkBoard + "/api/plugins/telemetry/" + "DEVICE" + "/" + id + "/values/timeseries?limit=100&agg=NONE&orderBy=DESC&useStrictDataTypes=false&keys=" + key + "&startTs=" + epochfirst + "&endTs=" + epochlast);
                var response = await httpClient.SendAsync(httpRequestMessage);
                responseContent = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseContent))
                {
                    responseContent = responseContent.Substring(responseContent.IndexOf("[") + 1).ToString();
                    responseContent = responseContent.Split("]")[0];
                    var data = responseContent.Split('}');
                    for (var i = 0; i <= data.Length; i++)
                    {
                        TimeSeriesValue timeSeriesValue = new TimeSeriesValue();
                        if (i == 0)
                        {
                            timeSeriesValue = JsonConvert.DeserializeObject<TimeSeriesValue>(data[i] + "}");
                        }
                        else
                        {
                            timeSeriesValue = JsonConvert.DeserializeObject<TimeSeriesValue>(data[i].Substring(1) + "}");
                        }
                        timeSeriesValue.time = Common.UnixTimeStampToDateTime(timeSeriesValue.ts);
                        timeSeriesValue.rownumber = i;
                        listTimeSeriesValue.Add(timeSeriesValue);
                    }
                }

                //listTimeSeriesValue.Add(chartInfo);
            }
            catch (Exception ex)
            {
            }
            ViewBag.TotalRecord = listTimeSeriesValue.Count() > 0 ? listTimeSeriesValue.Count() : 0;
            ViewBag.TotalPageByKey = (ViewBag.TotalRecord / 10) + 1;
            ViewBag.PageNumberByKey = pagenumber > 0 ? pagenumber : 1;
            var datares = listTimeSeriesValue.Where(x => x.rownumber >= ((pagenumber - 1) * 10) && x.rownumber <= (pagenumber * 10)).ToList();
            //ViewBag.PageFirstByKey = MonitorDatabaseFilter.pagefirst > 0 ? MonitorDatabaseFilter.pagefirst : 1;
            return Ok(new { data = datares });
        }
        public List<ChartInfo> GetListChartByMonitorStationID(int monitorStationID) //ds biểu đồ theo trạm
        {
            List<StationData> listStationData = new List<StationData>();
            listStationData = _iStationDataController.GetStationDataByMonitorStationID(monitorStationID);
            List<ChartInfo> listChartInfo = new List<ChartInfo>();
            foreach (var item in listStationData)
            {
                listChartInfo.Add(GetListChartByMonitorStationAndStationData(monitorStationID, item.stationDataId));
            }
            return listChartInfo;
        }
        public ChartInfo GetListChartByMonitorStationAndStationData(int monitorStationID, int stationDataID) //ds biểu đồ theo trạm và loai dl
        {
            List<int> listhour = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
            ChartInfo ChartInfo = new ChartInfo();
            var listmonitordatabase = _iMonitorDatabaseController.GetMonitorDatabaseByMonitorStationandStationDataAndTime(listhour, DateTime.Now, monitorStationID, stationDataID);
            ChartInfo.listvalue = listmonitordatabase.Select(x => x.monitorDatabaseValue).ToList();
            ChartInfo.monitorStationID = monitorStationID;
            ChartInfo.stationDataID = stationDataID;
            var stationData = _iStationDataController.GetStationDataByID(stationDataID);
            ChartInfo.chartName = "Biểu đồ " + stationData.stationDataName;
            ChartInfo.stationDataName = stationData.stationDataName;
            ChartInfo.listWarningMargin = _iWarningMarginController.GetWarningMarginByStationDataID(stationDataID);
            return ChartInfo;
        }

        public IActionResult ShowTemplate()
        {
            //ViewBag.Template=""
            return View("Template");
        }
        [HttpPost]
        public IActionResult GetListChartMulti([FromForm] string[] listmoniterstationid, int stationDataID, int type, string key)
        {

            List<ChartInfo> listChartInfo = new List<ChartInfo>();
            if (listmoniterstationid.Count() <= 0)
            {
                return Ok(new { data = "" });
            }
            if (type == (int)MethodType.THUCONG)
            {
                List<int> listid = new List<int>();
                listid = listmoniterstationid[0].Split(',').Select(x => Convert.ToInt32(x)).ToList();
                List<int> listhour = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
                foreach (var item in listid)
                {
                    var chartInfo = new ChartInfo();
                    chartInfo.listvalue = _iMonitorDatabaseController.GetMonitorDatabaseByMonitorStationandStationDataAndTime(listhour, DateTime.Now, item, stationDataID).Select(x => x.monitorDatabaseValue).ToList();
                    listChartInfo.Add(chartInfo);
                }
            }
            else
            {
                List<string> listidstring = new List<string>();
                listidstring = listmoniterstationid[0].Split(',').Select(x => x).ToList();
                foreach (var item in listidstring)
                {
                    var chartInfo = GetChartInfoByMoniterStationIDandStationDataIDFromApi(item, key, null, null).Result;
                    listChartInfo.Add(chartInfo);
                }

            }
            return Ok(new { data = listChartInfo });
        }
        public async Task<ChartInfo> GetChartInfoByMoniterStationIDandStationDataIDFromApi(string id, string key, DateTime? startDate, DateTime? endDate) // lấy dl từ api -dl trong ngày
        {
            List<int> listhour = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
            ChartInfo chartInfo = new ChartInfo();
            chartInfo.listvaluedouble = new List<double>();
            //chartInfo.monitorStationstringId = id;
            //chartInfo.stationDatakey = key;
            //chartInfo.chartName = "Biểu đồ " + key;
            //chartInfo.stationDataName = key;
            using var httpClient = new HttpClient();
            LoginHelper.LoginHelper.GetThinkBoardToken();
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml+json");
            httpClient.DefaultRequestHeaders.Add("X-Authorization", "bearer " + Startup.thinkportaccesstoken);
            foreach (var hour in listhour)
            {
                startDate = startDate == null ? null : DateTime.Now;
                endDate = endDate == null ? null : DateTime.Now;
                var epochfirst = (new DateTime(DateTime.Parse(startDate.Value.ToString()).Year, DateTime.Parse(startDate.Value.ToString()).Month, 26, hour, 00, 0, 0, System.DateTimeKind.Utc) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds * 1000;
                var epochlast = (new DateTime(DateTime.Parse(endDate.Value.ToString()).Year, DateTime.Parse(endDate.Value.ToString()).Month, 26, hour, 05, 0, 0, System.DateTimeKind.Utc) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds * 1000;
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                httpRequestMessage.RequestUri = new Uri(Startup.ConnectionStringsThinkBoard + "/api/plugins/telemetry/" + "DEVICE" + "/" + "b7379c80-df0f-11ea-959f-41472d299d8d" + "/values/timeseries?limit=100&agg=NONE&orderBy=DESC&useStrictDataTypes=false&keys=" + "no2" + "&startTs=" + /*epochfirst*/"1629936000000" + "&endTs=" + "1629936300000");
                var response = await httpClient.SendAsync(httpRequestMessage); ;
                responseContent = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseContent))
                {
                    var data = responseContent.Substring(responseContent.IndexOf("[") + 1).ToString();
                    data = data.Split("]")[0];
                    TimeSeriesValue timeSeriesValue = JsonConvert.DeserializeObject<TimeSeriesValue>(data);
                    chartInfo.listvaluedouble.Add(Convert.ToDouble(timeSeriesValue.value));
                }
            }
            return chartInfo;
        }
        [HttpPost]
        public void GetListAfterFilter(ChartInfoFilter chartInfoFilter)  //hàm gọi khi nhấn trong phần trang,lọc,...
        {
            TempData["ChartInfoFilter"] = JsonConvert.SerializeObject(chartInfoFilter);
        }
    }
}
