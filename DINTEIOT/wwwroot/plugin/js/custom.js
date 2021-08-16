/**
 *
 * You can write your JS code here, DO NOT touch the default style file
 * because it will make it harder for you to update.
 *
 */


// đăng xuất
$(".btnsignout").on('click', function () {

    $.ajax({
        url: '/account/logout',
        type: 'POST',
        async: false,
        success: function () {
            location.replace("/login");
        }
    });

});
