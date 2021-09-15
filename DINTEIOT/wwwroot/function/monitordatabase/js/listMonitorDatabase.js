let monitorDatabaseid;
let monitorDatabaseTime;
let monitorDatabaseValue;
let monitorDatabaseUnit;

let formMonitorDatabase;
var listMonitorDatabaseindex = {
    listMonitorDatabaseindex_init: function () {
        listMonitorDatabaseindex_init();
    }
};
function listMonitorDatabaseindex_init() {

    //sau khi button save change được ấn
    $('#btnsavesolieu').on('click', function () {
        debugger
        formMonitorDatabase = new FormData();
        monitorDatabaseid = $('#monitorDatabaseID').val();
        monitorDatabaseUnit = $('#monitorDatabaseUnit').val();
        monitorDatabaseTime = $('#monitorDatabaseTime').val();
        monitorDatabaseValue = $('#monitorDatabaseValue').val();
        stationDataID = $('#stationDataID').val();
        monitorStationID = $('#monitorStationID').val();
        //check phone number
        if (/*monitorDatabasename == '' || monitorDatabasename == undefined || monitorDatabasename == null*/
                stationDataID <= 0
            /*|| monitorDatabaseUnit == '' || monitorDatabaseUnit == undefined || monitorDatabaseUnit == null*/
            || monitorDatabaseTime == '' || monitorDatabaseTime == undefined || monitorDatabaseTime == null
            || monitorDatabaseValue == '' || monitorDatabaseValue == undefined || monitorDatabaseValue == null
        )
        {
            toastrnotifywarning('Bạn đang để trống trường bắt buộc');
            return false;
        }

        //if (checkmonitorDatabasebyname(monitorDatabasename, monitorDatabaseid) == true) {
        //    toastrnotifywarning("Tên cơ quan đã được sử dụng");
        //    return false;
        //}
        //if (checkmonitorDatabasebycode(monitorDatabasecode, monitorDatabaseid) == true) {
        //    showErrorMessage("Mã cơ quan đã được sử dụng");
        //    return false;
        //}
        formMonitorDatabase.append('monitorDatabaseUnit', monitorDatabaseUnit);
        formMonitorDatabase.append('monitorDatabaseTime', formatdatetoyyyyMMdd(monitorDatabaseTime));
        formMonitorDatabase.append('monitorDatabaseValue', monitorDatabaseValue);
        formMonitorDatabase.append('stationDataID', stationDataID);
        formMonitorDatabase.append('monitorStationID', monitorStationID);
        if (monitorDatabaseid <= 0 || monitorDatabaseid == undefined) {
            insertmonitorDatabase();
        }
        else {
            formMonitorDatabase.append('monitorDatabaseid', monitorDatabaseid);
            updatemonitorDatabase();
        }

        return false
    });
  
}
// thêm mới monitorDatabase
function insertmonitorDatabase() {
    $.ajax({
        url: "/monitorDatabase/Insert",
        type: "POST",
        async: false,
        data: formMonitorDatabase,
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
                    window.location.replace("/monitorDatabase/ListMonitorDatabase?monitorID=" + monitorStationID);
                }, 3000);
            }
        }
    });
}
// update monitorDatabase
function updatemonitorDatabase() {
    $.ajax({
        url: "/monitorDatabase/Update",
        type: "POST",
        async: false,
        data: formMonitorDatabase,
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
                    window.location.replace("/monitorDatabase/ListMonitorDatabase?monitorID=" + monitorStationID);
                }, 3000);
            }

        }
    });
}




//xóa monitorDatabase
function deletemonitorDatabase() {
    $.ajax({
        url: "/monitorDatabase/Delete",
        type: "GET",
        async: false,
        data: { monitorDatabaseid: monitorDatabaseid },
        success: function (response) {
            debugger
            if (response.status <= 0) {
                showErrorMessage(response.message);
                return false
            }
            else {
                showSuccessMessage(response.message);
                setTimeout(function () {
                    window.location.replace("/monitorDatabase");
                }, 2000);
            }

        }
    });
}


//check tên cơ quan đã tồn tại chưa
function checkmonitorDatabasebyname(monitorDatabaseName, id) {
    var check;
    $.ajax({
        url: "/monitorDatabase/CheckmonitorDatabaseByName",
        type: "POST",
        async: false,
        data: { monitorDatabaseName: monitorDatabaseName, id: id },
        success: function (response) {
            check = response.data;
        }
    });
    return check;
}
//check mã cơ quan đã tồn tại chưa
function checkmonitorDatabasebycode(monitorDatabasecode, id) {
    var check;
    $.ajax({
        url: "/admin/monitorDatabase/CheckmonitorDatabaseByCode",
        type: "POST",
        async: false,
        data: { monitorDatabasecode: monitorDatabasecode, id: id },
        success: function (response) {
            check = response.data;
        }
    });
    return check;
}


//button thêm số liệu
function ShowCreateMonitorDatabase() {
    $('#monitorDatabaseID').val(0);
    $('#monitorDatabasemodaltitle').html("Thêm số liệu")
    $.ajax({
        url: "/monitorDatabase/insertpre", // lấy organ theo id
        async: false,
        type: "GET",
        success: function (response) {
            if (response.data <= 0) {
                toastrnotifyerror(response.message);
                return false;
            }
            else {
                $('#ModalSoLieuQuanTrac').modal("show");
            }
        }
    })
}
//button sửa số liệu
function ShowEditMonitorStation(id) {
    $('#monitorDatabaseID').val(id);
    $('#monitorDatabasemodaltitle').html("Sửa số liệu")
    $.ajax({
        url: "/monitorDatabase/updatepre?id=" + id,
        async: false,
        type: "GET",
        success: function (response) {
            if (response.status <= 0) {
                toastrnotifyerror(response.message);
                return false;
            }
            else {
                $("#monitorDatabaseUnit").val(response.data.monitorDatabaseUnit);
                $("#monitorDatabaseValue").val(response.data.monitorDatabaseValue);
                $("#monitorDatabaseTime").val(response.data.monitorDatabaseTime);
                $('#stationDataID').val(response.data.stationDataID);
                $(".select2-selection__placeholder").html(response.data.stationDataName)
                $('#ModalSoLieuQuanTrac').modal("show");
            }
        }
    })
}

//xóa dữ liệu
function DeleteMonitorDB(id) {
    monitorDatabaseid = id;
    showConfirmDeleteMessage(accountindex, deleteafter);
}
function deleteafter() {
    $.ajax({
        url: "/Monitordatabase/Delete",
        type: "GET",
        async: false,
        data: { id: monitorDatabaseid },
        success: function (response) {
            debugger
            if (response.status <= 0) {
                showErrorMessage(response.message);
                return false
            }
            else {
                showSuccessMessage(response.message);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            }

        }
    });
}
