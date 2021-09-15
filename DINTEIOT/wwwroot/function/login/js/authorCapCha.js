
var captcha = sliderCaptcha({
    id: 'captcha',
    repeatIcon: 'fa fa-redo',
    onSuccess: function () {
        var handler = setTimeout(function () {
            window.clearTimeout(handler);
            window.location.replace("/login/LoginAfter");
        }, 500);
    }
});