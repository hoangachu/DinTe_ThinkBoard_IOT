let organid;
let organname;
let organcode;
let organparentid;
let spokesmanid;
let address;
let phonenumber;
let fax;
let email;
let url;
let controlhierarchy;//cấp quản lý
let formDataorgan;
var organindex = {
    organindex_init: function () {
        organindex_init();
    }
};
function organindex_init() {

    $('.btnalert').on("click", function () {
        showConfirmDeleteMessage(articleindex, Show);
    });
  
    //sau khi button save change được ấn
    $('#btnsaveorgan').on('click', function () {
        formDataorgan = new FormData();
        organid = $('#organid').val();
        organname = $('#organname').val();
        address = $('#address').val();
        phonenumber = $('#phonenumber').val();
        organparentid = $('#selectparentid').val();
        email = $('#email').val();
        //check phone number
        if (organname == '' || organname == undefined || organname == null) {
            toastrnotifywarning('Bạn đang để trống trường bắt buộc');
            return false;
        }
     
        if (checkorganbyname(organname, organid) == true) {
            toastrnotifywarning("Tên cơ quan đã được sử dụng");
            return false;
        }
        //if (checkorganbycode(organcode, organid) == true) {
        //    showErrorMessage("Mã cơ quan đã được sử dụng");
        //    return false;
        //}
        formDataorgan.append('organname', organname);
        formDataorgan.append('organcode', organcode);
        formDataorgan.append('organparentid', organparentid);
        formDataorgan.append('spokesmanid', spokesmanid);
        formDataorgan.append('address', address);
        formDataorgan.append('phonenumber', phonenumber);
        formDataorgan.append('fax', fax);
        formDataorgan.append('email', email);
        formDataorgan.append('url', url);
        formDataorgan.append('controlhierarchyid', controlhierarchy);
        if (organid <= 0 || organid == undefined) {
            insertorgan();
        }
        else {
            formDataorgan.append('organid', organid);
            updateorgan();
        }

        return false
    });
    //tạo table organ dạng treegrid
    createtableorgan();
}
// thêm mới organ
function insertorgan() {
    $.ajax({
        url: "Organ/Insert",
        type: "POST",
        async: false,
        data: formDataorgan,
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
                    window.location.replace("/Organ");
                }, 3000);
            }
        }
    });
}
// update organ
function updateorgan() {
    $.ajax({
        url: "Organ/Update",
        type: "POST",
        async: false,
        data: formDataorgan,
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
                    window.location.replace("/Organ");
                }, 3000);
            }

        }
    });
}
function createtableorgan() {
    $.ajax({

        url: 'Organ/GetJsonDataTreeGrid',
        type: "Post",
        success: function (response) {

            // chuẩn bị dữ liệu
            jsondata = JSON.parse(response.data);
            console.log(jsondata);
            var source =
            {
                dataType: "json",
                dataFields: [
                    { name: 'ID', type: 'number' },
                    { name: 'ParentID', type: 'number' },
                    { name: 'Name', type: 'string' }
                ],
                hierarchy:
                {
                    keyDataField: { name: 'ID' },
                    parentDataField: { name: 'ParentID' }
                },
                id: 'ID',
                localData: jsondata
            };
            var dataAdapter = new $.jqx.dataAdapter(source);
            // gọi hàm tạo bảng
            redertableorgan(dataAdapter);
        }

    });


}
// tạo bảng treegrid với jqwidget
function redertableorgan(source) {
    $("#treegrid").jqxTreeGrid(
        {

            source: source,
            sortable: true,
            checkboxes: false,
            hierarchicalCheckboxes: true,
            ready: function () {
                $("#treegrid").jqxTreeGrid('expandRow', '2');
            },
            editable: true,
            editSettings: { saveOnPageChange: true, saveOnBlur: true, saveOnSelectionChange: false, cancelOnEsc: true, saveOnEnter: true, editOnDoubleClick: false, editOnF2: false },
            // called when jqxTreeGrid is going to be rendered.
            rendering: function () {
                // destroys all buttons.
                if ($(".editButtons").length > 0) {
                    $(".editButtons").jqxButton('destroy');
                }
                if ($(".deleteButtons").length > 0) {
                    $(".deleteButtons").jqxButton('destroy');
                }
            },
            // called when jqxTreeGrid is rendered.
            rendered: function () {
                if ($(".editButtons").length > 0) {
                    $(".deleteButtons").jqxButton();
                    $(".editButtons").jqxButton();

                    var editClick = function (event) {
                        var target = $(event.target);
                        // get button's value.
                        var value = target.val();
                        // get clicked row.
                        var rowKey = event.target.getAttribute('data-row');
                        if (value == "Tùy chọn") {
                            // begin edit.
                            $("#treegrid").jqxTreeGrid('beginRowEdit', rowKey);
                            target.parent().find('.deleteButtons').show();
                            target.val("Save");
                        }
                        else {
                            // end edit and save changes.
                            target.parent().find('.deleteButtons').hide();
                            target.val("Edit");
                            $("#treegrid").jqxTreeGrid('endRowEdit', rowKey);
                        }
                    }
                    $(".editButtons").on('click', function (event) {
                        debugger
                        organid = $(this).data('row'); // lấy giá trị của row
                        //gán các trá trị  cho các input field
                        $('#organid').val(organid); //gán các trá trị
                        $.ajax({
                            url: "Organ/updatepre", // lấy organ theo id
                            async: false,
                            type: "GET",
                            data: { id: organid },
                            success: function (response) {
                                if (response.status <= 0) {
                                    showErrorMessage(response.message);
                                    return false;
                                }
                                else if (response.data != null && response.status == 1) {
                                    $('#organmodaltitle').html("Sửa đơn vị");
                                    var data = JSON.parse(response.data);
                                    $('#organname').val(data.organname);
                                    $('#selectparentid').val(data.organparentid);
                                    $("#select2-selectparentid-container").html(data.organparentname)
                                    $('#email').val(data.email);
                                    $('#address').val(data.address);
                                    $('#phonenumber').val(data.phonenumber);
                                    $('#ModalAddorEditOrgan').show();
                                }
                            }
                        })
                    });

                    $(".deleteButtons").click(function (event) {
                        organid = $(this).data('row'); // lấy giá trị của row
                        showConfirmDeleteMessage(organindex, deleteorgan);
                        // end edit and cancel changes.
                        $("#treegrid").jqxTreeGrid('endRowEdit', organid, true);
                    });
                }
            },
            columns: [
                { text: 'Tên cơ quan', align: "center", dataField: 'Name', width: 1200 },

                {
                    text: '', cellsAlign: 'center', width: 220, align: "center", columnType: 'none', editable: false, sortable: false, dataField: null, cellsRenderer: function (row, column, value) {
                        // render custom column.
                        return "<button  class='btn btn-icon editButtons waves-effect' data-toggle='modal' data-target='#ModalAddorEditOrgan' data-row='" + row + "'><i class='far fa-edit' data-toggle='tooltip' data-placement='bottom' data-original-title='Sửa cơ quan'></i></button><button data-toggle='tooltip' data-placement='bottom'  data-original-title='Xóa cơ quan' href='' class='btn btn-icon deleteButtons' data-row='" + row + "' style='margin-left:5px'><i class='fas fa-trash'></i></button>";
                    }
                }

            ]

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


//check tên cơ quan đã tồn tại chưa
function checkorganbyname(organName, id) {
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
//check mã cơ quan đã tồn tại chưa
function checkorganbycode(organcode, id) {
    var check;
    $.ajax({
        url: "/admin/organ/CheckOrganByCode",
        type: "POST",
        async: false,
        data: { organcode: organcode, id: id },
        success: function (response) {
            check = response.data;
        }
    });
    return check;
}

//button thêm cơ quan được nhấn
function ShowCreate() {
    $('#organid').val(0);
    $('#organmodaltitle').html("Thêm đơn vị")
    $.ajax({
        url: "Organ/insertpre", // lấy organ theo id
        async: false,
        type: "GET",
        success: function (response) {
            if (response.data <= 0) {
                showErrorMessage(response.message);
                return false;
            }
            else {
                $('#ModalAddorEditOrgan').show();
            }
        }
    })
}