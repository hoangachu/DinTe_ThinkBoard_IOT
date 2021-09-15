let currentpage;
let pagesize;
let totalpage;
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
//pagination dc chon
$('.page').click(function () {
    debugger
    currentpage = parseInt($(this).data('li'));
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
    var dateTimeType = $('#TimeDurationSelect').val();  // loại thời gian
    var liststationdataid = $('#ChartType').val();
    type = $("#MonitorStationfilter > :selected").data('type');
    var GuiID = null;
    var liststationdatakey = null;
    if (type == THUCONG) { }
    else { GuiID = monitorStationID; liststationdatakey = liststationdataid; }
    currentpagename = $('#currentpagename').val();

    $.ajax({
        url: currentpagename + '/GetListAfterFilter',
        type: 'POST',
        async: false,
        data: {
            pagenumber: currentpage, startdate: formatdatetoyyyyMMdd(startdate), enddate: formatdatetoyyyyMMdd(enddate), txtsearch: txtsearch, stationDataID: stationDataID,
            organID: organID, monitorStationID: monitorStationID, dateTimeType: dateTimeType, liststationdatakey: liststationdatakey, GuiID: GuiID
        },
        success: function (response) {
            window.location.reload();
        }
    })
}
function AppendPagination(totalpage) {
    debugger
    var html = '';
    if (totalpage <= 1) {
        html = '<li class="page-item page" data-li="@i"><a class="page-link" href="#">1</a> </li>';
    }
    else {
        for (var item = 1; item <= (totalpage >= 3 ? 3 : totalpage); item++) {
            html += '<li class="page-item page" data-li="@i"><a class="page-link" href="#">'+item+'</a></li>'
        }
    }
    $('#pagination').append(
        //'<input id="pagenumberbykey" value="@ViewBag.PageNumberByKey" style="display:none" />' +
        //'<input id="pagefirstbykey" value="@ViewBag.PageFirstByKey" style="display:none" />' +
        //'<input id="currentpagenamebykey" value="@ViewBag.CurrentPageNameByKey" style="display:none" />' +
        '<ul class="pagination mb-0">' +

        '<li class="page-item disabled previouspagination">' +
        '<a class="page-link" href="#" tabindex="-1"><i class="fas fa-chevron-left"></i></a>' +
        '</li>'
        +
        html
        +
        '<li class="page-item nextpagination">'+
           '<a class="page-link" href="#"><i class="fas fa-chevron-right"></i></a>' +
        '</li>' +
        '</ul>'


    );
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
    //pagination dc chon
    $('.page').click(function () {
        debugger
        currentpage = parseInt($(this).data('li'));
        $('#currentpage').val(currentpage);
        Getdatawithfilter();
    });
    //các ô input lọc được nhập
    $('.inputfilter').on('change', function () {
        currentpage = DEFAULTPAGE;
        Getdatawithfilter();
    });

}