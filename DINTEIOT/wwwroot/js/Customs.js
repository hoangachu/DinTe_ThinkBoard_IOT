//format datetime yyyy/MM/dd
function formatdatetoyyyyMMdd(datetime) {
    if (datetime != undefined) {
        var split = datetime.split("/");
        return split[2] + "/" + split[1] + "/" + split[0]
    }
}
function GetToken() {
    var token = null;
    $.ajax({
        url: "/login/GetToken",
        type: "GET",
        async: false,
        data: {},
        success: function (response) {
            token = response.data;
        }
    });
    return token;
}
//format datetime to yyyy/mm/dd
function getdateformat(now) {
    debugger
    var isoDate = new Date(now).toISOString();
    var date = new Date(isoDate);
    date = date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate();
    return (date);
}
function getdateformatUTC(now) {
    debugger
    var isoDate = new Date(now).toISOString();
    var date = new Date(isoDate);
    date = Date.UTC(date.getFullYear(), (date.getMonth()), date.getDate());
    return (date);
}
function getcurrentday() {
    var curr = new Date; // get current date
    var first = curr.getDate() - curr.getDay(); // First day is the day of the month - the day of the week
    var fromdate = new Date(curr.setDate(first)).toUTCString();
    return fromdate;
}


setInterval(function () {

    /*alert(1)*/
}, 2000);