let stationDataCode;
let stationDataID;
let stationDataName;
let formStationData;
var stationdataindex = {
    stationdataindex_init: function () {
       stationdataindex_init();
    }
};
function stationdataindex_init() {

    $('.btnalert').on("click", function () {
        showConfirmDeleteMessage(articleindex, Show);
    });
  
    //sau khi button save change được ấn
    $('#btnsaveloaidulieu').on('click', function () {
        formStationData = new FormData();
        stationDataid = $('#stationDataId').val();
        stationDataName = $('#stationDataName').val();
        stationDataCode = $('#stationDataCode').val();
        //check phone number
        if (stationDataName == '' || stationDataName == undefined || stationDataName == null
            || stationDataCode == '' || stationDataCode == undefined || stationDataCode == null) {
            toastrnotifywarning('Bạn đang để trống trường bắt buộc');
            return false;
        }
     
        if (checkdatastationbycode(stationDataCode, stationDataid) == true) {
            toastrnotifywarning("Mã loại dữ liệu đã được sử dụng");
            return false;
        }
     
        formStationData.append('stationDataName', stationDataName);
        formStationData.append('stationDataCode', stationDataCode);

        if (stationDataid <= 0 || stationDataid == undefined) {
            insertStationData();
        }
        else {
            formStationData.append('stationDataid', stationDataid);
            updateStationData();
        }

        return false
    });
}
// thêm mới organ
function insertStationData() {
    $.ajax({
        url: "StationData/Insert",
        type: "POST",
        async: false,
        data: formStationData,
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
                    window.location.replace("/StationData");
                }, 3000);
            }
        }
    });
}
// update organ
function updateStationData() {
    $.ajax({
        url: "StationData/Update",
        type: "POST",
        async: false,
        data: formStationData,
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
                    window.location.replace("/StationData");
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


//check tên loại dl đã tồn tại chưa
function checkstationdatabyname(stationDataName, id) {
    var check;
    $.ajax({
        url: "/organ/CheckOrganByName",
        type: "POST",
        async: false,
        data: { organName: organName, id: id },
        success: function (response) {
            check = response.data;
        }
    });
    return check;
}
//check mã loại dl đã tồn tại chưa
function checkdatastationbycode(stationDataCode, id) {
    var check;
    $.ajax({
        url: "/stationdata/CheckStationDataByCode",
        type: "POST",
        async: false,
        data: { stationDataCode: stationDataCode, id: id },
        success: function (response) {
            check = response.data;
        }
    });
    return check;
}

//button thêm loại dl được nhấn
function ShowCreateStationData() {
    $('#stationDataId').val(0);
    $('#stationdatamodaltitle').html("Thêm loại dữ liệu")
    $.ajax({
        url: "stationData/insertpre", // lấy organ theo id
        async: false,
        type: "GET",
        success: function (response) {
            if (response.data <= 0) {
                toastrnotifyerror(response.message);
                return false;
            }
            else {
                $('#ModalAddorEditLoaiDuLieu').modal("show");
            }
        }
    })
}
//button sửa loại dl được nhấn
function ShowEditStationData(id) {
    $('#stationDataId').val(id);
    $('#stationdatamodaltitle').html("Sửa loại dữ liệu")
    $.ajax({
        url: "stationData/updatepre?id=" +id,
        async: false,
        type: "GET",
        success: function (response) {
            if (response.status <= 0) {
                toastrnotifyerror(response.message);
                return false;
            }
            else {
                $("#stationDataName").val(response.data.stationDataName);
                $("#stationDataCode").val(response.data.stationDataCode);
                $('#ModalAddorEditLoaiDuLieu').modal("show");
            }
        }
    })
}