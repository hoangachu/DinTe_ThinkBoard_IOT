let stationMethodID;
let stationMethodName;
let stationMethodCode;
let formstationMethod;
var stationMethodindex = {
    stationMethodindex_init: function () {
        stationMethodindex_init();
    }
};
function stationMethodindex_init() {

    $('.btnalert').on("click", function () {
        showConfirmDeleteMessage(articleindex, Show);
    });

    //sau khi button lưu được ấn
    $('#btnsavetramquantrac').on('click', function () {
        debugger
        formstationMethod = new FormData();
        stationMethodID = $('#stationMethodID').val();
        stationMethodName = $('#stationMethodName').val();
        stationMethodCode = $('#stationMethodCode').val();
        stationDataID = $('#stationDataID').val();
        description = $('#description').val();
        if (stationMethodName == '' || stationMethodName == undefined || stationMethodName == null
            || stationMethodCode == '' || stationMethodCode == undefined || stationMethodCode == null
            || stationDataID <= 0)
        {
            toastrnotifywarning('Bạn đang để trống trường bắt buộc');
            return false;
        }

        if (checkstationMethodbyCode(stationMethodCode, stationMethodID) == true) {
            toastrnotifywarning("Mã phương thức đã được sử dụng");
            return false;
        }

        formstationMethod.append('stationMethodName', stationMethodName);
        formstationMethod.append('stationMethodCode', stationMethodCode);
        formstationMethod.append('stationDataID', stationDataID);
        formstationMethod.append('description', description);
        if (stationMethodID <= 0 || stationMethodID == undefined) {
            insertstationMethod();
        }
        else {
            formstationMethod.append('stationMethodID', stationMethodID);
            updatestationMethod();
        }

        return false
    });
}

// thêm mới ngưỡng
function insertstationMethod() {
    $.ajax({
        url: "stationMethod/Insert",
        type: "POST",
        async: false,
        data: formstationMethod,
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
                    window.location.replace("/stationMethod");
                }, 3000);
            }
        }
    });
}
// update ngưỡng
function updatestationMethod() {
    $.ajax({
        url: "stationMethod/Update",
        type: "POST",
        async: false,
        data: formstationMethod,
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
                    window.location.replace("/stationMethod");
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
function checkstationMethodbyName(stationMethodName, id) {
    var check;
    $.ajax({
        url: "/stationMethod/CheckstationMethodByName",
        type: "POST",
        async: false,
        data: { stationMethodName: stationMethodName, id: id },
        success: function (response) {
            check = response.data;
        }
    });
    return check;
}
//check tên trạm đã tồn tại chưa
function checkstationMethodbyCode(stationMethodCode, id) {
    var check;
    $.ajax({
        url: "/stationMethod/CheckstationMethodByCode",
        type: "POST",
        async: false,
        data: { stationMethodCode: stationMethodCode, id: id },
        success: function (response) {
            check = response.data;
        }
    });
    return check;
}

//button thêm pt được nhấn
function ShowCreatestationMethod() {
    $('#stationMethodID').val(0);
    $('#stationMethodmodaltitle').html("Thêm phương thức quan trắc")
    $.ajax({
        url: "stationMethod/insertpre", // lấy organ theo id
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
//button sửa pt được nhấn
function ShowEditstationMethod(id) {
    $('#stationMethodID').val(id);
    $('#stationMethodmodaltitle').html("Sửa phương thức quan trắc")
    $.ajax({
        url: "stationMethod/updatepre?id=" + id,
        async: false,
        type: "GET",
        success: function (response) {
            if (response.status <= 0) {
                toastrnotifyerror(response.message);
                return false;
            }
            else {
                $("#stationMethodName").val(response.data.stationMethodName);
                $("#stationMethodCode").val(response.data.stationMethodCode);
                $("#stationMethodType").val(response.data.stationMethodType);
                $("#description").val(response.data.description);
                $('#stationDataID').val(response.data.stationDataID);
                $(".select2-selection__placeholder").html(response.data.stationDataName)
                $('#ModalAddorEditPTQuanTrac').modal("show");
                $('#btnsavetramquantrac').css('display', 'inline-block');
            }
        }
    })
}
//button xem pt được nhấn
function ShowStationMethod(id) {
    $('#stationMethodID').val(id);
    $('#stationMethodmodaltitle').html("Thông tin phương thức quan trắc")
    $.ajax({
        url: "stationMethod/updatepre?id=" + id,
        async: false,
        type: "GET",
        success: function (response) {
            if (response.status <= 0) {
                toastrnotifyerror(response.message);
                return false;
            }
            else {
                $("#stationMethodName").val(response.data.stationMethodName);
                $("#stationMethodCode").val(response.data.stationMethodCode);
                $("#stationMethodType").val(response.data.stationMethodType);
                $("#description").val(response.data.description);
                $('#stationDataID').val(response.data.stationDataID);
                $(".select2-selection__placeholder").html(response.data.stationDataName)
                $('#ModalAddorEditPTQuanTrac').modal("show");
                $('#btnsavetramquantrac').css('display', 'none');
            }
        }
    })
}