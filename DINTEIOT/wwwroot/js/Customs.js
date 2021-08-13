//format datetime yyyy/MM/dd
function formatdatetoyyyyMMdd(datetime) {
    if (datetime != undefined) {
        var split = datetime.split("/");
        return split[2] + "/" + split[1] + "/" + split[0]
    }
}