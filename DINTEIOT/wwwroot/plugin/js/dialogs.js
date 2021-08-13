window.onload = function () {

    //$('.btnalert').on('click', function () {
    //    debugger
    //    var type = $(this).data('type');
    //    if (type === 'basic') {
    //        showBasicMessage();
    //    }
    //    else if (type === 'with-title') {
    //        showWithTitleMessage();
    //    }
    //    else if (type === 'success') {
    //        showSuccessMessage();
    //    }
    //    else if (type === 'confirm') {
    //        showConfirmMessage();
    //    }
    //    else if (type === 'confirmdelete') {
    //        showConfirmDeleteMessage();
    //    } 
    //    else if (type === 'cancel') {
    //        showCancelMessage();
    //    }
    //    else if (type === 'with-custom-icon') {
    //        showWithCustomIconMessage();
    //    }
    //    else if (type === 'html-message') {
    //        showHtmlMessage();
    //    }
    //    else if (type === 'autoclose-timer') {
    //        showAutoCloseTimerMessage();
    //    }
    //    else if (type === 'prompt') {
    //        showPromptMessage();
    //    }
    //    else if (type === 'ajax-loader') {
    //        showAjaxLoaderMessage();
    //    }
    //});
};
//These codes takes from http://t4t5.github.io/sweetalert/
function showBasicMessage() {
    swal("Here's a message!");
}

function showWithTitleMessage() {
    swal("Here's a message!", "It's pretty, isn't it?");
}
//message hiển thị thành công.Ấn OK thì load lại trang hiện tại
function showSuccessMessage(message) {
    swal({
        title: message,
        text: "",
        type: "success",
        showCancelButton: false,
        confirmButtonColor: "#8cd4f5",
        confirmButtonText: "OK",
        closeOnConfirm: false
    });
}
//message hiển thị thành công.Ấn OK thì chuyển trang // áp dụng cho TH có nút cancel (hủy thao tác)
function showSuccessMessageWithRedirect(message, urlredirect) {
    swal({
        title: message,
        text: "",
        type: "success",
        showCancelButton: true,
        confirmButtonColor: "#8cd4f5",
        confirmButtonText: "OK!",
        closeOnConfirm: false
    }, function () {
        window.location.replace(urlredirect)
    });
}
function showErrorMessage(message) {
    swal({
        title: message,
        text: "",
        type: "warning",
        showCancelButton: false,
        confirmButtonColor: "#8cd4f5",
        confirmButtonText: "OK",
        closeOnConfirm: false
    });
    //}, function () {
    //    location.reload().delay(2000);
    //});
}
function showConfirmMessage(namespace, callback) {
    swal({
        title: "Are you sure?",
        text: "You will not be able to recover this imaginary file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    }, function () {
        swal("Deleted!", "Your imaginary file has been deleted.", "success");
        namespace.callback();
    });
}
function showConfirmInsertPost(namespace, insertpost, insertdraft) {
    swal({
        title: "Are you sure?",
        text: "Hãy chọn cách bạn muốn lưu!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Xuất bản",
        cancelButtonText: "Lưu bản nháp",
        closeOnConfirm: false,
        closeOnCancel: false
    },
        function (isConfirm) {
            if (isConfirm) {
                insertpost();
                /*swal("OK!", "Thêm bài viết thành công", "success");*/
            } else {
                insertdraft();
                /* swal("OK", "Lưu bản nháp thành công", "success");*/
            }
        });
}
function showConfirmDeleteMessage(namespace, callback) {
    swal({
        title: "Bạn có chắc không?",
        text: "Khi chọn có dữ liệu của bạn sẽ bị xóa!",
        type: "warning",
        showCancelButton: true,
        cancelButtonText : "Không",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Vâng, xóa nó!",
        closeOnConfirm: false
    }, function () {
        callback();
        /*swal("Đã xóa!", "", "success");*/
    });
}
function showConfirmBlockAccountMessage(namespace, callback) {
    swal({
        title: "Bạn có chắc không?",
        text: "Khi chọn có tài khoản sẽ bị khóa!",
        type: "warning",
        showCancelButton: true,
        cancelButtonText: "Không",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Vâng, tôi đồng ý!",
        closeOnConfirm: false
    }, function () {
        swal("Đã khóa tài khoản!", "", "success");
        callback();
    });
}
function showConfirmSelectOptionMessage(namespace, insertpost, insertdraft) {
  
}

function showCancelMessage() {
    swal({
        title: "Are you sure?",
        text: "You will not be able to recover this imaginary file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "No, cancel plx!",
        closeOnConfirm: false,
        closeOnCancel: false
    }, function (isConfirm) {
        if (isConfirm) {
            swal("Deleted!", "Your imaginary file has been deleted.", "success");
        } else {
            swal("Cancelled", "Your imaginary file is safe :)", "error");
        }
    });
}

function showWithCustomIconMessage() {
    swal({
        title: "Sweet!",
        text: "Here's a custom image.",
        imageUrl: "../../images/thumbs-up.png"
    });
}

function showAlertMessage(message) {
    swal({
        title: "Thông Báo!",
        text: message,
        closeOnConfirm: false,
    });
}

function showAutoCloseTimerMessage() {
    swal({
        title: "Auto close alert!",
        text: "I will close in 2 seconds.",
        timer: 2000,
        showConfirmButton: false
    });
}

function showPromptMessage() {
    swal({
        title: "An input!",
        text: "Write something interesting:",
        type: "input",
        showCancelButton: true,
        closeOnConfirm: false,
        animation: "slide-from-top",
        inputPlaceholder: "Write something"
    }, function (inputValue) {
        if (inputValue === false) return false;
        if (inputValue === "") {
            swal.showInputError("You need to write something!"); return false
        }
        swal("Nice!", "You wrote: " + inputValue, "success");
    });
}

function showAjaxLoaderMessage() {
    swal({
        title: "Ajax request example",
        text: "Submit to run ajax request",
        type: "info",
        showCancelButton: true,
        closeOnConfirm: false,
        showLoaderOnConfirm: true,
    }, function () {
        setTimeout(function () {
            swal("Ajax request finished!");
        }, 2000);
    });
}
function showHtmlMessage() {
    swal({
        title: "HTML <small>Title</small>!",
        text: "A custom <span style=\"color: #CC0000\">html<span> message.",
        html: true
    });
}
function showAutoCloseTimerMessage() {
    swal({
        title: "Auto close alert!",
        text: "I will close in 2 seconds.",
        timer: 4000,
        showConfirmButton: false
    });
}
