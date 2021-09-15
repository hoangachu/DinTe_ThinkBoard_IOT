let warningMarginID;
let warningMarginValueFrom;
let warningMarginValueTo;
let warningMarginValueUnit;
let warningMarginValueColor;
let warningMarginName;
let formwarningMargin;
var warningMarginindex = {
    warningMarginindex_init: function () {
        warningMarginindex_init();
    }
};
function warningMarginindex_init() {

    $('.btnalert').on("click", function () {
        showConfirmDeleteMessage(articleindex, Show);
    });

    //sau khi button lưu được ấn
    $('#btnsavenguongcanhbao').on('click', function () {
        formwarningMargin = new FormData();
        warningMarginid = $('#warningMarginId').val();
        warningMarginName = $('#warningMarginName').val();
        warningMarginValueFrom = $('#warningMarginValueFrom').val();
        stationDataID = $('#stationDataID').val();
        warningMarginValueTo = $('#warningMarginValueTo').val();
        warningMarginValueUnit = $('#warningMarginValueUnit').val();
        warningMarginValueColor = $('#warningMarginValueColor').val();
        if (warningMarginName == '' || warningMarginName == undefined || warningMarginName == null
            || warningMarginValueFrom <= 0
            || warningMarginValueTo <= 0
            || warningMarginValueColor == '' || warningMarginValueColor == undefined || warningMarginValueColor == null
            || stationDataID <= 0        ) {
            toastrnotifywarning('Bạn đang để trống trường bắt buộc');
            return false;
        }

        if (checkwarningMarginbyname(warningMarginName, warningMarginid) == true) {
            toastrnotifywarning("Tên ngưỡng cảnh báo đã được sử dụng");
            return false;
        }

        formwarningMargin.append('warningMarginName', warningMarginName);
        formwarningMargin.append('warningMarginValueFrom', warningMarginValueFrom);
        formwarningMargin.append('stationDataID', stationDataID);
        formwarningMargin.append('warningMarginValueTo', warningMarginValueTo);
        formwarningMargin.append('warningMarginValueUnit', warningMarginValueUnit);
        formwarningMargin.append('warningMarginValueColor', warningMarginValueColor);
        if (warningMarginid <= 0 || warningMarginid == undefined) {
            insertwarningMargin();
        }
        else {
            formwarningMargin.append('warningMarginid', warningMarginid);
            updatewarningMargin();
        }

        return false
    });
}

// thêm mới ngưỡng
function insertwarningMargin() {
    $.ajax({
        url: "warningMargin/Insert",
        type: "POST",
        async: false,
        data: formwarningMargin,
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
                    window.location.replace("/warningMargin");
                }, 3000);
            }
        }
    });
}
// update ngưỡng
function updatewarningMargin() {
    $.ajax({
        url: "warningMargin/Update",
        type: "POST",
        async: false,
        data: formwarningMargin,
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
                    window.location.replace("/warningMargin");
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


//check tên ngưỡng đã tồn tại chưa
function checkwarningMarginbyname(warningMarginName, id) {
    var check;
    $.ajax({
        url: "/warningMargin/CheckWarningMarginByName",
        type: "POST",
        async: false,
        data: { warningMarginName: warningMarginName, id: id },
        success: function (response) {
            check = response.data;
        }
    });
    return check;
}
//check mã loại dl đã tồn tại chưa
//function checkdatastationbycode(warningMarginCode, id) {
//    var check;
//    $.ajax({
//        url: "/warningMargin/CheckwarningMarginByCode",
//        type: "POST",
//        async: false,
//        data: { warningMarginCode: warningMarginCode, id: id },
//        success: function (response) {
//            check = response.data;
//        }
//    });
//    return check;
//}

//button thêm ngưỡng được nhấn
function ShowCreateWarningMargin() {
    $('#warningMarginId').val(0);
    $('#warningmarginmodaltitle').html("Thêm ngưỡng cảnh báo")
    $.ajax({
        url: "warningMargin/insertpre", // lấy organ theo id
        async: false,
        type: "GET",
        success: function (response) {
            if (response.data <= 0) {
                toastrnotifyerror(response.message);
                return false;
            }
            else {
                $('#ModalAddorEditNguongCanhBao').modal("show");
            }
        }
    })
}
//button sửa ngưỡng được nhấn
function ShowEditWarningMargin(id) {
    $('#warningMarginId').val(id);
    $('#warningmarginmodaltitle').html("Sửa ngưỡng cảnh báo")
    $.ajax({
        url: "warningMargin/updatepre?id=" + id,
        async: false,
        type: "GET",
        success: function (response) {
            if (response.status <= 0) {
                toastrnotifyerror(response.message);
                return false;
            }
            else {
                $("#warningMarginName").val(response.data.warningMarginName);
                $("#warningMarginValueFrom").val(response.data.warningMarginValueFrom);
                $("#warningMarginValueTo").val(response.data.warningMarginValueTo);
                $("#warningMarginValueColor").val(response.data.warningMarginValueColor);
                $("#warningMarginValueUnit").val(response.data.warningMarginValueUnit); 
                $("#warningMarginID").val(response.data.warningMarginID);
                $('#stationDataID').val(response.data.stationDataID);
                $(".select2-selection__placeholder").html(response.data.stationDataName)
                $('#ModalAddorEditNguongCanhBao').modal("show");
            }
        }
    })
}