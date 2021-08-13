let accountID;
let userName;
let fullName;
let phoneNumber;
var accountindex = {
    accountindex_init: function () {
        accountindex_init();
    }
};
function accountindex_init() {

    $('.btnalert').on("click", function () {
       
    });

    //sau khi button lưu được ấn
    $('#btnsavenguoidung').on('click', function () {
        debugger
        formaccount = new FormData();
        accountID = $('#accountID').val();
        userName = $('#userName').val();
        fullName = $('#fullName').val();
        phoneNumber = $('#phoneNumber').val();
        organID = $('#selectparentid').val();
        email = $('#email').val();
        if (userName == '' || userName == undefined || userName == null
            || fullName == '' || fullName == undefined || fullName == null) {
            toastrnotifywarning('Bạn đang để trống trường bắt buộc');
            return false;
        }

        if (checkaccountbyUserName(userName, accountID) == true) {
            toastrnotifywarning("Tên tài khoản đã được sử dụng");
            return false;
        }

        formaccount.append('userName', userName);
        formaccount.append('fullName', fullName);
        formaccount.append('phoneNumber', phoneNumber);
        formaccount.append('organID', organID);
        formaccount.append('email', email);
        if (accountID <= 0 || accountID == undefined) {
            insertaccount();
        }
        else {
            formaccount.append('accountID', accountID);
            updateaccount();
        }

        return false
    });
}

// thêm mới tk
function insertaccount() {
    $.ajax({
        url: "account/Insert",
        type: "POST",
        async: false,
        data: formaccount,
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
                    window.location.replace("/account");
                }, 3000);
            }
        }
    });
}
// update tk
function updateaccount() {
    $.ajax({
        url: "account/Update",
        type: "POST",
        async: false,
        data: formaccount,
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
                    window.location.replace("/account");
                }, 3000);
            }

        }
    });
}


//xóa người dùng
function Delete(id) {
    accountID = id;
    showConfirmDeleteMessage(accountindex, deleteafter);
}
function deleteafter() {
    $.ajax({
        url: "/Account/Delete",
        type: "GET",
        async: false,
        data: { id: accountID },
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

//check tên tk đã tồn tại chưa
function checkaccountbyUserName(userName, id) {
    var check;
    $.ajax({
        url: "/account/CheckAccountByUserName",
        type: "POST",
        async: false,
        data: { userName: userName, id: id },
        success: function (response) {
            check = response.data;
        }
    });
    return check;
}


//button thêm tk được nhấn
function ShowCreateaccount() {
    $('#accountID').val(0);
    $('#Accountmodaltitle').html("Thêm người dùng")
    $.ajax({
        url: "account/insertpre", // lấy organ theo id
        async: false,
        type: "GET",
        success: function (response) {
            if (response.data <= 0) {
                toastrnotifyerror(response.message);
                return false;
            }
            else {
                $('#ModalAddorEditNguoiDung').modal("show");
            }
        }
    })
}
//button sửa tk được nhấn
function ShowEditaccount(id) {
    $('#accountID').val(id);
    $('#Accountmodaltitle').html("Thêm người dùng")
    $.ajax({
        url: "account/updatepre?id=" + id,
        async: false,
        type: "GET",
        success: function (response) {
            if (response.status <= 0) {
                toastrnotifyerror(response.message);
                return false;
            }
            else {
                $("#userName").val(response.data.username);
                $("#fullName").val(response.data.fullName);
                $("#phoneNumber").val(response.data.phoneNumber);
                $("#email").val(response.data.email);
                if (response.data.organ != null) {
                    $('#selectparentid').val(response.data.organ.organid);
                    $("#select2-selectparentid-container").html(response.data.organ.organname)
                }
               
                $('#ModalAddorEditNguoiDung').modal("show");
            }
        }
    })
}
//hàm cập nhật trạng thái người dùng
function SetAccountStatus(id) {
    var check = 0;
    if ($("#ck_" + id).is(':checked')) {
        check = 1;
    }
    $.ajax({
        url: "account/updateAccountStatus?id=" + id + "&&status=" + check,
        async: false,
        type: "GET",
        success: function (response) {
            if (response.status <= 0) {
                toastrnotifyerror(response.message);
                return false;
            }
            else {
                toastrnotifysuccess(response.message);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            }
        }
    })
}