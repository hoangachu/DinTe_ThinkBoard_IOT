let currentpage;
let pagesize;
let DEFAULTPAGE = 1;

//button xem trang trước được nhấn (phân trang)
$('.previouspagination').click(function () {
    currentpage = parseInt($('#currentpage').val()) - 1;
    $('#currentpage').val(currentpage);
    Getdatawithfilter();

});
//button xem trang sau được nhấn (phân trang)
$('.nextpagination').click(function () {
    currentpage = parseInt($('#currentpage').val()) + 1;
    $('#currentpage').val(currentpage);
    Getdatawithfilter();
});

//các ô input lọc được nhập
$('.inputfilter').on('change', function () {
    currentpage = DEFAULTPAGE;
    Getdatawithfilter();
});

//hàm lấy dữ liệu cùng với các điều kiện lọc
function Getdatawithfilter() {
    var startdate = $('#startdate').val();
    var enddate = $('#enddate').val();
    var txtsearch = $('#txtsearch').val();
    stationDataID = $('#stationDataIDfilter').val();  // lấy loại dữ liệu quan trắc từ ô filter_select để lọc
    var organID = $('#OrganIDfilter').val();  // lấy cơ quan để lọc
    var monitorStationID = $('#MonitorStationfilter').val();  // lấy trạm quan trắc để lọc
    currentpagename = $('#currentpagename').val();
 
    $.ajax({
        url: currentpagename + '/GetListAfterFilter',
        type: 'POST',
        async: false,
        data: {
            pagenumber: currentpage, startdate: formatdatetoyyyyMMdd(startdate), enddate: formatdatetoyyyyMMdd(enddate), txtsearch: txtsearch, stationDataID: stationDataID,
            organID: organID, monitorStationID: monitorStationID
        },
        success: function (response) {
            window.location.reload();
        }
    })
}