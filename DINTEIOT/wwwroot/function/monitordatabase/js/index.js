let monitorDatabasename;
let monitorDatabasecode;
let monitorDatabaseparentid;
let formDatamonitorDatabase;
var monitorDatabaseindex = {
    monitorDatabaseindex_init: function () {
        monitorDatabaseindex_init();
    }
};
function monitorDatabaseindex_init() {

    $('#tabletram > tbody > tr > td').click(function () {
        var id = $(this).data('id');
        ShowData(id);
    });

    //sau khi button save change được ấn
    $('#btnsavemonitorDatabase').on('click', function () {
        formDatamonitorDatabase = new FormData();
        monitorDatabaseid = $('#monitorDatabaseid').val();
        monitorDatabasename = $('#monitorDatabasename').val();
        address = $('#address').val();
        phonenumber = $('#phonenumber').val();
        monitorDatabaseparentid = $('#selectparentid').val();
        email = $('#email').val();
        //check phone number
        if (monitorDatabasename == '' || monitorDatabasename == undefined || monitorDatabasename == null) {
            toastrnotifywarning('Bạn đang để trống trường bắt buộc');
            return false;
        }

        if (checkmonitorDatabasebyname(monitorDatabasename, monitorDatabaseid) == true) {
            toastrnotifywarning("Tên cơ quan đã được sử dụng");
            return false;
        }
        //if (checkmonitorDatabasebycode(monitorDatabasecode, monitorDatabaseid) == true) {
        //    showErrorMessage("Mã cơ quan đã được sử dụng");
        //    return false;
        //}
        formDatamonitorDatabase.append('monitorDatabasename', monitorDatabasename);
        formDatamonitorDatabase.append('monitorDatabasecode', monitorDatabasecode);
        formDatamonitorDatabase.append('monitorDatabaseparentid', monitorDatabaseparentid);
        formDatamonitorDatabase.append('spokesmanid', spokesmanid);
        formDatamonitorDatabase.append('address', address);
        formDatamonitorDatabase.append('phonenumber', phonenumber);
        formDatamonitorDatabase.append('fax', fax);
        formDatamonitorDatabase.append('email', email);
        formDatamonitorDatabase.append('url', url);
        formDatamonitorDatabase.append('controlhierarchyid', controlhierarchy);
        if (monitorDatabaseid <= 0 || monitorDatabaseid == undefined) {
            insertmonitorDatabase();
        }
        else {
            formDatamonitorDatabase.append('monitorDatabaseid', monitorDatabaseid);
            updatemonitorDatabase();
        }

        return false
    });
   
}
// thêm mới monitorDatabase
function insertmonitorDatabase() {
    $.ajax({
        url: "monitorDatabase/Insert",
        type: "POST",
        async: false,
        data: formDatamonitorDatabase,
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
                    window.location.replace("/monitorDatabase");
                }, 3000);
            }
        }
    });
}
// update monitorDatabase
function updatemonitorDatabase() {
    $.ajax({
        url: "monitorDatabase/Update",
        type: "POST",
        async: false,
        data: formDatamonitorDatabase,
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
                    window.location.replace("/monitorDatabase");
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
                }, 3000);
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

//button thêm cơ quan được nhấn
function ShowData(id) {
    $.ajax({
        url: "monitorDatabase/showdatapre", // lấy monitorDatabase theo id
        async: false,
        type: "GET",
        success: function (response) {
            if (response.data <= 0) {
                showErrorMessage(response.message);
                return false;
            }
            else {
                window.location.replace("/monitorDatabase/ListMonitorDatabase?monitorID=" + id);
            }
        }
    })
}