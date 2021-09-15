let monitorStationID;
let monitorStationName;
let monitorStationCode;
let longitude;
let latitude;
let siteAddress;
let description;
let formMonitorStation;
let listStationDataId = [];
let hasNext = true;
let TUDONG = 2;
let THUCONG = 1;
let type;
var monitorStationindex = {
    monitorStationindex_init: function () {
        monitorStationindex_init();
    }
};
function monitorStationindex_init() {


    //sau khi button lưu được ấn
    $('#btnsavetramquantrac').on('click', function () {
        debugger
        formMonitorStation = new FormData();
        monitorStationID = $('#monitorStationId').val();
        monitorStationName = $('#monitorStationName').val();
        monitorStationCode = $('#monitorStationCode').val();
        longitude = Number($('#longitude').val().replace(/[^0-9\.]+/g, ""));
        latitude = $('#latitude').val();
        siteAddress = $('#siteAddress').val();
        address = $('#address').val();
        organid = $('#selectparentid').val();
        description = $('#description').val();
        listStationDataId = $('#stationdataid').val();
        if (monitorStationName == '' || monitorStationName == undefined || monitorStationName == null
            || monitorStationCode == '' || monitorStationCode == undefined || monitorStationCode == null
            || longitude <= 0
            || latitude <= 0
            || organid <= 0
            || listStationDataId.length <= 0
        ) {
            toastrnotifywarning('Bạn đang để trống trường bắt buộc');
            return false;
        }

        if (checkmonitorStationbyCode(monitorStationCode, monitorStationID) == true) {
            toastrnotifywarning("Mã trạm đã được sử dụng");
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
        formMonitorStation.append('listStationDataId', listStationDataId);
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
//lay ds tram theo api
function GetStatoionDataFromTN(page) {
    debugger
    var token = GetToken();
    $.ajax({
        url: "http://10.1.10.66:8080/api/tenant/deviceInfos?pageSize=20&page=" + page,
        type: "GET",
        beforeSend: function (request) {
            request.setRequestHeader("X-Authorization", "bearer " + token);
        },
        data: {},
        success: function (response) {
            if (response.data.length > 0) {
                type = TUDONG;
                deletestationdata(0, TUDONG); // xóa tất cả loại dl lấy từ api
                deletestationdata_MonitorStation();
                for (var item = 0; item < response.data.length; item++) {
                    formMonitorStation = new FormData();
                    formMonitorStation.append('monitorStationName', response.data[item].name);
                    formMonitorStation.append('monitorStationCode', response.data[item].label);
                    formMonitorStation.append('GuiID', response.data[item].id.id);
                    formMonitorStation.append('type', type);
                    //formMonitorStation.append('longitude', response.data[i].name);
                    //formMonitorStation.append('latitude', latitude);
                    //formMonitorStation.append('siteAddress', siteAddress);
                    //formMonitorStation.append('address', address);
                    //formMonitorStation.append('description', description);
                    //formMonitorStation.append('organid', organid);
                    //formMonitorStation.append('listStationDataId', listStationDataId);
                    insertmonitorStation();
                    var data = GetStationDatabyStationFromApi(response.data[item].id.id); //lấy ds loại dl from api
                    SaveStationDatabyStationFromApi(data, response.data[item].id.id);
                }
            }
            hasNext = response.hasNext;
            if (hasNext == true) {
                page += 1;
                GetStatoionDataFromTN(page);
            }
        }

    });
}
// thêm mới ngưỡng
function insertmonitorStation() {
    $.ajax({
        url: "/monitorStation/Insert",
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
            else if (type == TUDONG) {

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
        url: "/monitorStation/Update",
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


//xóa 
function deletemonitorstation(id, type) {
    $.ajax({
        url: "/MonitorStation/Delete",
        type: "GET",
        async: false,
        data: { id: id, type: type },
        success: function (response) {
            debugger
            if (response.status <= 0) {
                showErrorMessage(response.message);
                return false
            }
            else {
                //showSuccessMessage(response.message);
                //setTimeout(function () {
                //    window.location.replace("/Organ");
                //}, 3000);
            }

        }
    });
}


//check mã trạm đã tồn tại chưa
function checkmonitorStationbyCode(monitorStationCode, id) {
    var check;
    $.ajax({
        url: "/monitorStation/CheckMonitorStationByCode",
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
//function checkdatastationbycode(monitorStationCode, id) {
//    var check;
//    $.ajax({
//        url: "/monitorStation/CheckmonitorStationByCode",
//        type: "POST",
//        async: false,
//        data: { monitorStationCode: monitorStationCode, id: id },
//        success: function (response) {
//            check = response.data;
//        }
//    });
//    return check;
//}

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
                $(".select2-selection__placeholder").html(response.data.organName);
                $('#loaidl > span > span > span > ul').find('.select2-selection__choice').remove();
                for (var i = 0; i < response.data.listStationData.length; i++) {
                    listStationDataId.push(response.data.listStationData[i].stationDataId);
                    $('#loaidl > span > span > span > ul').prepend('<li class="select2-selection__choice" title = "' + response.data.listStationData[i].stationDataName + '" onclick="remove(this,' + response.data.listStationData[i].stationDataId + ')"> <span class="select2-selection__choice__remove" role="presentation">×</span>' + response.data.listStationData[i].stationDataName + '</li>')
                }
                $("#stationdataid").val(listStationDataId);
                $('#ModalAddorEditTramQuanTrac').modal("show");


            }
        }
    })
}
function remove(el, id) {
    debugger
    var element = el;
    element.remove();
    var array = [];
    debugger
    array = listStationDataId;
    listStationDataId = [];
    for (var i = 0; i <= array.length; i++) {
        if (array[i] != id && array[i] != undefined) {
            listStationDataId.push(array[i]);
        }
    }
    $("#stationdataid").val(listStationDataId);
}

//thêm dl vào bảng MonitorStation_StationData dl từ api
function insertMonitorStation_StationDataFromApi(guiId, key) {
    $.ajax({
        url: "/monitorStation/InsertToMonitorStation_StationData_FromApi",
        type: "POST",
        async: false,
        data: { guiId: guiId, key: key },
        success: function (response) {
        }
    });
}