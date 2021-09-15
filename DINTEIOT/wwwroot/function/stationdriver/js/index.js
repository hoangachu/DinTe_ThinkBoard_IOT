let stationDriverID;
let stationDriverName;
let stationDriverCode;
let stationDriverType;
let formStationDriver;
var stationDriverindex = {
    stationDriverindex_init: function () {
        stationDriverindex_init();
    }
};
function stationDriverindex_init() {

    $('.btnalert').on("click", function () {
        showConfirmDeleteMessage(articleindex, Show);
    });

    //sau khi button lưu được ấn
    $('#btnsavetramquantrac').on('click', function () {
        debugger
        formStationDriver = new FormData();
        stationDriverID = $('#stationDriverID').val();
        stationDriverName = $('#stationDriverName').val();
        stationDriverCode = $('#stationDriverCode').val();
        stationDriverType = $('#stationDriverType').val();
        monitorStationID = $('#monitorStationID').val();
        description = $('#description').val();
        if (stationDriverName == '' || stationDriverName == undefined || stationDriverName == null
            || stationDriverCode == '' || stationDriverCode == undefined || stationDriverCode == null)
        {
            toastrnotifywarning('Bạn đang để trống trường bắt buộc');
            return false;
        }

        if (checkstationDriverbyCode(stationDriverCode, stationDriverID) == true) {
            toastrnotifywarning("Mã phương tiện đã được sử dụng");
            return false;
        }

        formStationDriver.append('stationDriverName', stationDriverName);
        formStationDriver.append('stationDriverCode', stationDriverCode);
        formStationDriver.append('monitorStationID', monitorStationID);
        formStationDriver.append('stationDriverType', stationDriverType);
        formStationDriver.append('description', description);
        if (stationDriverID <= 0 || stationDriverID == undefined) {
            insertstationDriver();
        }
        else {
            formStationDriver.append('stationDriverID', stationDriverID);
            updatestationDriver();
        }

        return false
    });
}

// thêm mới ngưỡng
function insertstationDriver() {
    $.ajax({
        url: "stationDriver/Insert",
        type: "POST",
        async: false,
        data: formStationDriver,
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
                    window.location.replace("/stationDriver");
                }, 3000);
            }
        }
    });
}
// update ngưỡng
function updatestationDriver() {
    $.ajax({
        url: "stationDriver/Update",
        type: "POST",
        async: false,
        data: formStationDriver,
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
                    window.location.replace("/stationDriver");
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
function checkstationDriverbyCode(stationDriverCode, id) {
    var check;
    $.ajax({
        url: "/stationDriver/CheckstationDriverByName",
        type: "POST",
        async: false,
        data: { stationDriverCode: stationDriverCode, id: id },
        success: function (response) {
            check = response.data;
        }
    });
    return check;
}
//check tên trạm đã tồn tại chưa
function checkstationDriverbycode(stationDriverCode, id) {
    var check;
    $.ajax({
        url: "/stationDriver/CheckstationDriverByCode",
        type: "POST",
        async: false,
        data: { stationDriverCode: stationDriverCode, id: id },
        success: function (response) {
            check = response.data;
        }
    });
    return check;
}

//button thêm trạm được nhấn
function ShowCreatestationDriver() {
    $('#stationDriverID').val(0);
    $('#stationDrivermodaltitle').html("Thêm phương tiện quan trắc")
    $.ajax({
        url: "stationDriver/insertpre", // lấy organ theo id
        async: false,
        type: "GET",
        success: function (response) {
            if (response.data <= 0) {
                toastrnotifyerror(response.message);
                return false;
            }
            else {
                $('#ModalAddorEditPTQuanTrac').modal("show");
            }
        }
    })
}
//button sửa trạm được nhấn
function ShowEditstationDriver(id) {
    $('#stationDriverID').val(id);
    $('#stationDrivermodaltitle').html("Sửa phương tiện quan trắc")
    $.ajax({
        url: "stationDriver/updatepre?id=" + id,
        async: false,
        type: "GET",
        success: function (response) {
            if (response.status <= 0) {
                toastrnotifyerror(response.message);
                return false;
            }
            else {
                $("#stationDriverName").val(response.data.stationDriverName);
                $("#stationDriverCode").val(response.data.stationDriverCode);
                $("#stationDriverType").val(response.data.stationDriverType);
                $("#description").val(response.data.description);
                $('#monitorStationID').val(response.data.monitorStationID);
                $(".select2-selection__placeholder").html(response.data.monitorStationName)
                $('#ModalAddorEditPTQuanTrac').modal("show");
            }
        }
    })
}