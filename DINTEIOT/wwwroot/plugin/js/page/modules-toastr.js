"use strict";

$("#toastr-1").click(function() {
  iziToast.info({
    title: 'Hello, world!',
    message: 'This awesome plugin is made iziToast toastr',
    position: 'topRight'
  });
});

$("#toastr-2").click(function() {
  iziToast.success({
    title: 'Hello, world!',
    message: 'This awesome plugin is made by iziToast',
    position: 'topRight'
  });
});

$("#toastr-3").click(function() {
  iziToast.warning({
    title: 'Hello, world!',
    message: 'This awesome plugin is made by iziToast',
    position: 'topRight'
  });
});

$("#toastr-4").click(function() {
  iziToast.error({
    title: 'Hello, world!',
    message: 'This awesome plugin is made by iziToast',
    position: 'topRight'
  });
});

$("#toastr-5").click(function() {
  iziToast.show({
    title: 'Hello, world!',
    message: 'This awesome plugin is made by iziToast',
    position: 'bottomRight' 
  });
});

$("#toastr-6").click(function() {
  iziToast.show({
    title: 'Hello, world!',
    message: 'This awesome plugin is made by iziToast',
    position: 'bottomCenter' 
  });
});

$("#toastr-7").click(function() {
  iziToast.show({
    title: 'Hello, world!',
    message: 'This awesome plugin is made by iziToast',
    position: 'bottomLeft' 
  });
});

$("#toastr-8").click(function() {
  iziToast.show({
    title: 'Hello, world!',
    message: 'This awesome plugin is made by iziToast',
    position: 'topCenter' 
  });
});

//thông báo thông tin
function toastrnotifyinfo(text) {
    iziToast.info({
        title: 'Thông tin !',
        message: text,
        position: 'topRight'
    });
}
//thông báo thành công
function toastrnotifysuccess(text) {
    iziToast.success({
        title: "Thành công !",
        message: text,
        position: 'topRight'
    });
}
//thông báo lỗi
function toastrnotifyerror(text) {
    iziToast.error({
        title: 'Lỗi !',
        message: text,
        position: 'topRight'
    });
}
//thông báo cảnh báo
function toastrnotifywarning(text) {
    iziToast.warning({
        title: 'Cảnh báo !',
        message: text,
        position: 'topRight'
    });
}
//thong bao
function toastrnotifyshow(text) {
    iziToast.show({
        title: 'Thông báo!',
        message: text,
        position: 'topRight'
    });
}