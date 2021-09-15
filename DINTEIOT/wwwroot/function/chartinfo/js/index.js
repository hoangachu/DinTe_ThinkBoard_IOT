let CHECK = 1;
let liststationdataid = [];
var chartinfoindex = {
    chartinfoindex_init: function () {
        chartinfoindex_init();
    }
};
function chartinfoindex_init() {
    $("#ChartType").prop("disabled", true);
    //make gg chart location
    /*  L.map('map').setView([51.505, -0.09], 13);*/

    google.charts.load('current', {
        'packages': ['map'],
        // Note: you will need to get a mapsApiKey for your project.
        // See: https://developers.google.com/chart/interactive/docs/basic_load_libs#load-settings
        'mapsApiKey': 'AIzaSyD57PRHJQSQ5XQOuNtAWpRBOP-UCX5pSzA'
    });
    google.charts.setOnLoadCallback(drawMap);


    function drawMap() {
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Address');
        data.addColumn('string', 'Location');
        data.addColumn('string', 'Marker');
        $.ajax({
            url: '/ChartInfo/GetMonitorStation',
            type: 'GET',
            data: {},
            success: function (response) {
                debugger
                if (response.data.length > 0) {
                    var location = [];
                    for (var item = 0; item < response.data.length; item++) {
                        var maker = [];
                        maker.push(response.data[item].longitude + ',' + response.data[item].latitude, response.data[item].monitorStationName, 'normal');
                        location.push(maker)
                    }
                    data.addRows(location);
                }
            }
        });

        var url = 'https://icons.iconarchive.com/icons/icons-land/vista-map-markers/48/';

        var options = {
            zoomLevel: 5,
            showTooltip: true,
            showInfoWindow: true,
            useMapTypeControl: true,
            icons: {
                normal: {
                    normal: url + 'Map-Marker-Ball-Pink-icon.png',
                    selected: url + 'Map-Marker-Push-Pin-1-Right-Chartreuse-icon.png'
                },
                selected: {
                    normal: url + 'Map-Marker-Push-Pin-1-Right-Chartreuse-icon.png',
                    selected: url + 'Map-Marker-Ball-Pink-icon.png'
                }
            },
            mapType: 'Map'
        };
        var map = new google.visualization.Map(document.getElementById('map'));

        map.draw(data, options);
    }
    GetDataFirst();
}

function CheckSelected() {
    var monitorstationid = $("#MonitorStationfilter").val();
    type = $("#MonitorStationfilter > :selected").data('type');
    var datetype = $('#TimeDurationSelect').val();
    if (monitorstationid >= 0 || monitorstationid != undefined || monitorstationid != null) {
        $("#ChartType").prop("disabled", false);

        if (type == THUCONG) {
            $("#ChartType").find('option').remove();
            //ds loai dl theo tram
            $.ajax({
                url: '/StationData/GetStationDataByMonitorStationID',
                type: "GET",
                data: { id: monitorstationid },
                success: function (response) {
                    $('#ChartType').append($('<option>',
                        {
                            value: 0,
                            text: "Tất cả"
                        }));
                    for (i = 0; i < response.length; i++) {
                        $('#ChartType').append($('<option>',
                            {
                                value: response[i].stationDataId,
                                text: response[i].stationDataName
                            }));
                    }
                }
            });
            renderChart(monitorstationid, 0);
            //ds dl theo thoi gian
            GetListDBByDate(monitorstationid, 0);
        }

        else {
            $("#ChartType").find('option').remove();
            $("#ChartTypev1").find('option').remove();
            $.ajax({
                url: '/ChartInfo/GetTimesSeriesKey',
                type: "GET",
                async: false,
                data: { id: monitorstationid },
                success: function (response) {
                    var data = JSON.parse(response.data);
                    $('#ChartType').append($('<option>',
                        {
                            value: 0,
                            text: "Tất cả"
                        }));
                    $('#ChartTypev1').append($('<option>',
                        {
                            value: 0,
                            text: "Tất cả"
                        }));
                    $("#listchart").html('');

                    for (i = 0; i < data.length; i++) {
                        $('#ChartType').append($('<option>',
                            {
                                value: data[i],
                                text: data[i]
                            }));
                        $('#ChartTypev1').append($('<option>',
                            {
                                value: data[i],
                                text: data[i]
                            }));
                    }
                }

            });

        }
    }
}
function CheckSelectLoaiBieuDo() {
    var stationDataID = $("#ChartType").val();
    var monitorstationid = $("#MonitorStationfilter").val();
    type = $("#MonitorStationfilter > :selected").data('type');
    if (type == THUCONG) {
        renderChart(monitorstationid, stationDataID);
    }
    else {
        $("#listchart").html('');
        renderChartforauto(monitorstationid, stationDataID)
    }
}
function GetDataFirst() {
    debugger
    console.log(liststationdataid)
    for (var i = 0; i < liststationdataid.length; i++) {
        $("#ChartType").prop("disabled", false);
      /*  listStationDataId.push(liststationdataid[i].stationDataId);*/
        $('#ChartTypediv > span > span > span > ul').prepend('<li class="select2-selection__choice" style="display: flex;" title = "' + liststationdataid[i] + '" onclick="removenew(this)"> <span class="select2-selection__choice__remove" role="presentation" style="display:block !important">×</span>' + liststationdataid[i] + '</li>')
    }
    $("#ChartType").val(liststationdataid);
    GetData();
}
function removenew(el) {
    debugger
    var element = el;
    element.remove();
    var array = [];
    debugger
    array = liststationdataid;
    liststationdataid = [];
    for (var i = 0; i <= array.length; i++) {
        if (array[i] != undefined) {
            liststationdataid.push(array[i]);
        }
    }
    $("#ChartType").val(liststationdataid);
}
//
function GetData() {
    debugger
    var monitorstationid = $("#MonitorStationfilter").val();
    type = $("#MonitorStationfilter > :selected").data('type');
    var liststationdataidnew = liststationdataid.length > 0 ? liststationdataid : $('#ChartType').val();
    if (type == THUCONG) { }
    else {
        if (liststationdataidnew.length > 0) {
            $("#listchart").html('');
            totalpage = 0;
            for (var item = 0; item < liststationdataidnew.length; item++) {
                renderChartforauto(monitorstationid, liststationdataidnew[item], type)
            }
            GetLastDataFromAPI(monitorstationid);
            AppendPagination(totalpage);
        }
    }
}
//
function CreateTemplate() {
    $('.main-sidebar').css('display', 'none');
    $('.main-navbar').css('display', 'none');
    $('.headertilte').css('display', 'none');
    $('#main-content').removeClass('main-content');
    var temHtml = $('#main-content').html();
}
//thucong
function renderChart(monitorstationid, stationDataID) {
    //chart
    $("#listchart").html('');
    $.ajax({
        url: "/ChartInfo/GetListChart",
        type: "GET",
        data: { monitorStationID: monitorstationid, stationDataID: stationDataID },
        success: function (response) {
            if (response.data.length > 0) {
                for (var item = 0; item < response.data.length; item++) {
                    $("#listchart").append('<div class="card gradient-bottom">' +
                        '<div class="card-header card-header-modal">' +
                        '<h4>' +
                        response.data[item].chartName +
                        '</h4>' +
                        '</div>' +
                        '<div class="card-body" id="top-5-scroll" tabindex="2" style="height: 315px; overflow: hidden; outline: none;">' +
                        '<figure class="highcharts-figure">' +
                        '<div id="chart_' + response.data[item].stationDataID + '"></div>' +
                        '<p class="62-description">' +

                        '</p>' +
                        '</figure>' +
                        '</div>' +
                        '<div class="card-footer pt-3 d-flex justify-content-center card-footer_chart">' +
                        '</div>');

                    for (var i = 0; i < response.data[item].listWarningMargin.length; i++) {

                        $('.card-footer_chart').append(
                            '<div class="budget-price justify-content-center">' +
                            '<div class="budget-price-square " data-width="20" style="width: 20px;background-color:' + response.data[item].listWarningMargin[i].warningMarginValueColor + '"></div>' +
                            '<div class="budget-price-label">' + response.data[item].listWarningMargin[i].warningMarginValueFrom + '-' + response.data[item].listWarningMargin[i].warningMarginValueTo + '</div>' +
                            '</div>'

                        )

                    }


                    Highcharts.chart('chart_' + response.data[item].stationDataID, {
                        chart: {
                            type: 'column'
                        },
                        title: {
                            text: 'Biểu đồ dữ liệu quan trắc trong ngày'
                        },
                        subtitle: {
                            text: ''
                        },
                        xAxis: {
                            categories: [
                                '00h',
                                '01h',
                                '02h',
                                '03h',
                                '04h',
                                '05h',
                                '06h',
                                '07h',
                                '08h',
                                '09h',
                                '10h',
                                '11h',
                                '12h',
                                '13h',
                                '14h',
                                '15h',
                                '16h',
                                '17h',
                                '18h',
                                '19h',
                                '20h',
                                '21h',
                                '22h',
                                '23h'
                            ],
                            crosshair: true
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                /*text: 'Rainfall (mm)'*/
                            }
                        },
                        tooltip: {
                            crosshairs: true,
                            shared: true,
                            valueSuffix: '°C'
                        },
                        plotOptions: {
                            column: {
                                pointPadding: 0.001,
                                borderWidth: 0
                            }
                        },
                        series: [{
                            name: response.data[item].stationDataName,
                            data: response.data[item].listvalue,
                            color: CHECK == 1 ? 'red' : 'red'
                        }]
                    });

                }
            }
            else {
                $("#listchart").append(
                    '<div class="card carddefault">' +
                    '<div class="card-body">' +
                    '<i>Không có dữ liệu</i>' +
                    '</div>' +
                    '</div>')
            }
        }
    });
}
//tudong
function renderChartforauto(id, key, type) {
    //chart
    var datetype = $('#TimeDurationSelect').val();
    var startDate = null;
    var endDate = null;
    endDate = getcurrentday();
    let datetypearr = [];
    if (datetype == 1) {
        var curr = new Date; // get current date
        var first = curr.getDate() - curr.getDay();
        var last = first - 6; // last day is the first day + 6
        fromdate = new Date(curr.setDate(last)).toUTCString();
        /*datetypearr = ['Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7', 'Chủ nhật']*/
    }
    else if (datetype == 2) {
        var today = new Date();
        fromdate = new Date().setDate(today.getDate() - 30);
        /*for (var item = fromdate.getDate(); item <= endDate.)*/
    }
    else {
        var today = new Date();
        fromdate = new Date().setDate(today.getDate() - 90);
    }

    $.ajax({
        url: "/ChartInfo/GetDataByStationIDandKeys",
        type: "GET",
        async: false,
        data: { id: id, key: key, startDate: startDate, endDate: endDate, typedatetime: datetype },
        success: function (response) {
            debugger
            var data = [];
            //if (response.data.length > 0) {
            //    for (var item = 0; item < response.data.length; item++) {
            if (response.data.listTimeSeriesValue.length > 0) {
               
                for (var item = 0; item < response.data.listTimeSeriesValue.length; item++) {
                    debugger
                    var obj = {
                        y: response.data.listTimeSeriesValue[item].valuedouble,
                        x: getdateformatUTC(response.data.listTimeSeriesValue[item].time)
                    };
                    data.push(obj);
                }
            }
            $("#listchart").append('<div class="card gradient-bottom">' +
                '<div class="card-header card-header-modal">' +
                '<h4>' +
                response.data.chartName +
                '</h4>' +
                '</div>' +
                '<div class="card-body" id="top-5-scroll" tabindex="2" style="height: 315px; overflow: hidden; outline: none;">' +
                '<figure class="highcharts-figure">' +
                '<div id="chart_' + response.data.stationDatakey + '"></div>' +
                '<p class="highcharts-description">' +

                '</p>' +
                '</figure>' +
                '</div>' +
                '<div class="card-footer pt-3 d-flex justify-content-center card-footer_chart">' +
                '</div>');

            Highcharts.chart('chart_' + response.data.stationDatakey, {
                chart: {
                    type: 'column'
                },
                title: {
                    text: ''
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    type: 'datetime',
                    dateTimeLabelFormats: {
                        day: '%e/%m/%y'
                    }
                },
                yAxis: {
                   /* min: 0,*/
                    title: {
                        /*text: 'Rainfall (mm)'*/
                    }
                },
                tooltip: {
                    crosshairs: true,
                    shared: true,
                    valueSuffix: ''
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.01,
                        borderWidth: 0
                    }
                },
                series: [{
                    data: data, name: response.data.stationDatakey
                }]
            });
            /*}*/
            /*}*/
            //else {
            //    $("#listchart").append(
            //        '<div class="card carddefault">' +
            //        '<div class="card-body">' +
            //        '<i>Không có dữ liệu</i>' +
            //        '</div>' +
            //        '</div>')
            //}
        }
    });
    GetListDBByDateAuto(id, key);
  
   
}
//lấy dữ liệu mới nhất từ api
function GetLastDataFromAPI(id) {
    $.ajax({
        url: "/ChartInfo/GetLastTimeSeries",
        type: "GET",
        async: false,
        data: { id: id},
        success: function (response) {
            var data = JSON.parse(response.data);
            $('#tbodylastdata').html('');
            if (data != null) {
                var obj = null;
                Object.getOwnPropertyNames(data).forEach(
                    function (val, idx, array) {
                        obj = data[val];
                        $('#tbodylastdata').append('<tr>' +
                            '<td>' + val + '</td>' +
                            '<td>' + obj[0].value + '</td>' +
                            '<td>' + ('chưa có') + '</td>' +
                            '<td>' + obj[0].ts + '</td>' +
                            '</tr>'
                        )
                    }
                );
            }
        }
    });
}

function GetListDBByDatePre() {
    var monitorstationid = $("#MonitorStationfilter").val();
    var stationDataID = $("#ChartType").val();
    type = $("#MonitorStationfilter > :selected").data('type');
    if (type == THUCONG) {
        GetListDBByDate(monitorstationid, stationDataID);
    }
    else {
        $('#tbody').html('')
        if (stationDataID == 0) {
            $.ajax({
                url: '/ChartInfo/GetTimesSeriesKey',
                type: "GET",
                async: false,
                data: { id: monitorstationid },
                success: function (response) {
                    var data = JSON.parse(response.data);
                    for (i = 0; i < data.length; i++) {
                        GetListDBByDateAuto(monitorstationid, data[i])
                    }
                }
            });
        }
        else {
            GetListDBByDateAuto(monitorstationid, stationDataID) // stationDataID = key
        }

    }
}
//lay ds du lieu cho table
function GetListDBByDate(monitorstationid, stationDataId) {
    var datetype = $('#TimeDurationSelect').val();
    debugger
    var fromdate = null;
    var endDate = null;
    endDate = getcurrentday();
    if (datetype == 1) {
        var curr = new Date; // get current date
        var first = curr.getDate() - curr.getDay();
        var last = first - 6; // last day is the first day + 6
        fromdate = new Date(curr.setDate(last)).toUTCString();
    }
    else if (datetype == 2) {
        var today = new Date();
        fromdate = new Date().setDate(today.getDate() - 30);
    }
    else {
        var today = new Date();
        fromdate = new Date().setDate(today.getDate() - 90);
    }
    var formdatachart = new FormData();
    formdatachart.append('stationDataID', stationDataId)
    formdatachart.append('monitorStationID', monitorstationid)
    formdatachart.append('startdate', getdateformat(fromdate))
    formdatachart.append('endDate', getdateformat(endDate))
    $.ajax({
        url: '/MonitorDatabase/GetMonitorDatabaseByMonitorStationandStationDataAndDate',
        type: "POST",
        data: formdatachart,
        contentType: false,
        processData: false,
        async: false,
        success: function (responses) {
            $('#tbody').html('')
            if (responses.length > 0) {
                $('#trdefault').css('display', 'none');
                for (var i = 0; i < responses.length; i++) {
                    $('#tbody').append('<tr>' +
                        '<td>' + responses[i].monitorDatabaseTime + '</td>' +
                        '<td>' + responses[i].stationDataName + '</td>' +
                        '<td>' + responses[i].monitorDatabaseValue + '</td>' +
                        '</tr>'
                    )
                }

            }
        }
    });
}

//lay ds du lieu cho table
function GetListDBByDateAuto(id, key) {
    debugger
    var datetype = $('#TimeDurationSelect').val();
    var startDate = null;
    var endDate = null;
    endDate = getcurrentday();
    if (datetype == 1) {
        var curr = new Date; // get current date
        var first = curr.getDate() - curr.getDay();
        var last = first - 6; // last day is the first day + 6
        startDate = new Date(curr.setDate(last)).toUTCString();
    }
    else if (datetype == 2) {
        var today = new Date();
        startDate = new Date().setDate(today.getDate() - 30);
    }
    else {
        var today = new Date();
        startDate = new Date().setDate(today.getDate() - 90);
    }

    $.ajax({
        url: '/ChartInfo/GetDataByStationIDandKeysAndDate',
        type: "POST",
        data: { id: id, key: key, startDate: getdateformat(startDate), endDate: getdateformat(endDate) },
        async: false,
        success: function (responsev2) {
            debugger
          
            if (responsev2.data.length > 0) {
                totalpage += responsev2.data.length;
                $('#trdefault').css('display', 'none');
                for (var i = 0; i < responsev2.data.length; i++) {
                    if (responsev2.data[i].value != null && responsev2.data[i].value != undefined) {
                        $('#tbody').append('<tr>' +
                            '<td>' + responsev2.data[i].time + '</td>' +
                            '<td>' + key + '</td>' +
                            '<td>' + responsev2.data[i].value + '</td>' +
                            '</tr>'
                        )
                    }

                }

            }
        }
    });
 
}

//
function CheckSelectLoaiBieuDoMulti() {
    var stationdataid = $("#ChartTypeMulti").val();
    var id = null;
    var key = null;
    type = $("#ChartTypeMulti > :selected").data('type');
    if (type == THUCONG) { id = stationdataid }
    else { key = stationdataid }
    $.ajax({
        url: '/MonitorStation/GetAllListMonitorStationByStationDataID',
        type: "GET",
        data: { id: id, type: type, key: key },
        success: function (response) {
            $('#MonitorStationfilter').append($('<option>',
                {
                    value: 0,
                    text: "Tất cả"
                }));
            for (i = 0; i < response.length; i++) {
                if (type == THUCONG) {
                    $('#MonitorStationfilterMulti').append($('<option>',
                        {
                            value: response[i].monitorStationID,
                            text: response[i].monitorStationName
                        }));
                }
                else {
                    $('#MonitorStationfilterMulti').append($('<option>',
                        {
                            value: response[i].GuiID,
                            text: response[i].monitorStationName
                        }));
                }

            }
        }
    });
}

// 
function CheckSelectedMulti() {
    debugger
    var listmoniterstationid = $('#MonitorStationfilterMulti').val();
    var stationdataid = $("#ChartTypeMulti").val();
    type = $("#ChartTypeMulti > :selected").data('type');
    renderChartmulti(listmoniterstationid, stationdataid, type);
}
//
function renderChartmulti(listmoniterstationid, stationdataid, type) {
    var id = null;
    var key = null;
    if (type == 1) { id = stationdataid }
    else { key = stationdataid }
    //chart
    $("#listchart").html('');
    var formdata = new FormData();
    formdata.append('listmoniterstationid', listmoniterstationid);
    formdata.append('stationDataID', id);
    formdata.append('type', type);
    formdata.append('key', key);
    $.ajax({
        url: "/ChartInfo/GetListChartMulti",
        type: "POST",
        async: false,
        data: formdata,
        contentType: false,
        processData: false,
        success: function (response) {
            debugger
            if (response.data.length > 0) {

                var listdata = [];
                for (var i = 0; i < response.data.length; i++) {
                    var item = { name: response.data[i].monitorStationName, data: type == THUCONG ? response.data[i].listvalue : response.data[i].listvaluedouble };
                    listdata.push(item);
                }
                Highcharts.chart('listchart', {
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: 'Biểu đồ dữ liệu quan trắc trong ngày'
                    },
                    subtitle: {
                        text: ''
                    },
                    xAxis: {
                        categories: [
                            '00h',
                            '01h',
                            '02h',
                            '03h',
                            '04h',
                            '05h',
                            '06h',
                            '07h',
                            '08h',
                            '09h',
                            '10h',
                            '11h',
                            '12h',
                            '13h',
                            '14h',
                            '15h',
                            '16h',
                            '17h',
                            '18h',
                            '19h',
                            '20h',
                            '21h',
                            '22h',
                            '23h'
                        ],
                        crosshair: true
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: ''
                        }
                    },
                    tooltip: {
                        crosshairs: true,
                        shared: true,
                        valueSuffix: ''
                    },
                    plotOptions: {
                        column: {
                            pointPadding: 0.2,
                            borderWidth: 0
                        }
                    },
                    series: listdata
                });
            }
            else {
                $("#listchart").append(
                    '<div class="card carddefault">' +
                    '<div class="card-body">' +
                    '<i>Không có dữ liệu</i>' +
                    '</div>' +
                    '</div>')
            }
        }
    });
}
//lấy ds key by trạm quan trắc từ api
function GetKeySeriesFromApi(monitorstationid) {
    var data = null;
    $.ajax({
        url: '/ChartInfo/GetTimesSeriesKey',
        type: "GET",
        async: false,
        data: { id: monitorstationid },
        success: function (response) {
             data = JSON.parse(response.data);
        }

    });
    return data;
}
//lọc loại dl
function GetDataTableFilter() {
    debugger
    var stationDataID = $("#ChartTypev1").val();
    var monitorstationid = $("#MonitorStationfilter").val();
    type = $("#MonitorStationfilter > :selected").data('type');
    if (type == THUCONG) {

    }
    else {
        $('#tbody').html('');
        GetListDBByDateAuto(monitorstationid, stationDataID);
    
    }
}