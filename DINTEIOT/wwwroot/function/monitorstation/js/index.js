let monitorStationID;
let monitorStationName;
let monitorStationCode;
let longitude;
let latitude;
let siteAddress;
let description;
let formMonitorStation;
var monitorStationindex = {
    monitorStationindex_init: function () {
        monitorStationindex_init();
    }
};
function monitorStationindex_init() {

    $('.btnalert').on("click", function () {
        showConfirmDeleteMessage(articleindex, Show);
    });

    //sau khi button lưu được ấn
    $('#btnsavetramquantrac').on('click', function () {
        formMonitorStation = new FormData();
        monitorStationID = $('#monitorStationId').val();
        monitorStationName = $('#monitorStationName').val();
        monitorStationCode = $('#monitorStationCode').val();
        longitude = $('#longitude').val();
        latitude = $('#latitude').val();
        siteAddress = $('#siteAddress').val();
        address = $('#address').val();
        organid = $('#selectparentid').val();
        description = $('#description').val();
        if (monitorStationName == '' || monitorStationName == undefined || monitorStationName == null
            || monitorStationCode == '' || monitorStationCode == undefined || monitorStationCode == null
            || longitude <= 0
            || latitude <= 0
            || organid <= 0)
        {
            toastrnotifywarning('Bạn đang để trống trường bắt buộc');
            return false;
        }

        if (checkmonitorStationbyCode(monitorStationCode, monitorStationID) == true) {
            toastrnotifywarning("Tên ngưỡng cảnh báo đã được sử dụng");
            return false;
        }

        formMonitorStation.append('monitorStationName', monitorStationName);
        formMonitorStation.append('monitorStationCode', monitorStationCode);
        formMonitorStation.append('longitude', longitude);
        formMonitorStation.append('latitude', latitude);
        formMonitorStation.append('siteAddress', siteAddress);
        formMonitorStation.append('address', address);
        formMonitorStation.append('description', description);
        formMonitorStation.append('organid', organid);
        if (monitorStationID <= 0 || monitorStationID == undefined) {
            insertmonitorStation();
        }
        else {
            formMonitorStation.append('monitorStationid', monitorStationID);
            updatemonitorStation();
        }

        return false
    });
}

// thêm mới ngưỡng
function insertmonitorStation() {
    $.ajax({
        url: "monitorStation/Insert",
        type: "POST",
        async: false,
        data: formMonitorStation,
        contentType: false,
        processData: false,
        success: function (response) {
            $('.btnhidemodal').click();
            if (response.status <= 0) {
                toastrnotifyerror(response.message)
                return false;
            }
            else {
                toastrnotifysuccess(response.message);
                setTimeout(function () {
                    window.location.replace("/monitorStation");
                }, 3000);
            }
        }
    });
}
// update ngưỡng
function updatemonitorStation() {
    $.ajax({
        url: "monitorStation/Update",
        type: "POST",
        async: false,
        data: formMonitorStation,
        contentType: false,
        processData: false,
        success: function (response) {
            $('.btnhidemodal').click();
            if (response.status <= 0) {
                toastrnotifyerror(response.message)
                return false;
            }
            else {
                toastrnotifysuccess(response.message);
                setTimeout(function () {
                    window.location.replace("/monitorStation");
                }, 3000);
            }

        }
    });
}


//xóa organ
function deleteorgan() {
    $.ajax({
        url: "/Organ/Delete",
        type: "GET",
        async: false,
        data: { organid: organid },
        success: function (response) {
            debugger
            if (response.status <= 0) {
                showErrorMessage(response.message);
                return false
            }
            else {
                showSuccessMessage(response.message);
                setTimeout(function () {
                    window.location.replace("/Organ");
                }, 3000);
            }

        }
    });
}


//check mã trạm đã tồn tại chưa
function checkmonitorStationbyCode(monitorStationCode, id) {
    var check;
    $.ajax({
        url: "/monitorStation/CheckmonitorStationByName",
        type: "POST",
        async: false,
        data: { monitorStationCode: monitorStationCode, id: id },
        success: function (response) {
            check = response.data;
        }
    });
    return check;
}
//check tên trạm đã tồn tại chưa
function checkdatastationbycode(monitorStationCode, id) {
    var check;
    $.ajax({
        url: "/monitorStation/CheckmonitorStationByCode",
        type: "POST",
        async: false,
        data: { monitorStationCode: monitorStationCode, id: id },
        success: function (response) {
            check = response.data;
        }
    });
    return check;
}

//button thêm trạm được nhấn
function ShowCreatemonitorStation() {
    $('#monitorStationId').val(0);
    $('#monitorstationmodaltitle').html("Thêm trạm quan trắc")
    $.ajax({
        url: "monitorStation/insertpre", // lấy organ theo id
        async: false,
        type: "GET",
        success: function (response) {
            if (response.data <= 0) {
                toastrnotifyerror(response.message);
                return false;
            }
            else {
                $('#ModalAddorEditTramQuanTrac').modal("show");
            }
        }
    })
}
//button sửa trạm được nhấn
function ShowEditmonitorStation(id) {
    $('#monitorStationId').val(id);
    $('#monitorstationmodaltitle').html("Sửa trạm quan trắc")
    $.ajax({
        url: "monitorStation/updatepre?id=" + id,
        async: false,
        type: "GET",
        success: function (response) {
            if (response.status <= 0) {
                toastrnotifyerror(response.message);
                return false;
            }
            else {
                $("#monitorStationName").val(response.data.monitorStationName);
                $("#monitorStationCode").val(response.data.monitorStationCode);
                $("#longitude").val(response.data.longitude);
                $("#latitude").val(response.data.latitude);
                $("#siteAddress").val(response.data.siteAddress);
                $("#address").val(response.data.address); 
                $("#description").val(response.data.description);
                $('#selectparentid').val(response.data.organID);
                $(".select2-selection__placeholder").html(response.data.organName)
                $('#ModalAddorEditTramQuanTrac').modal("show");
            }
        }
    })
}