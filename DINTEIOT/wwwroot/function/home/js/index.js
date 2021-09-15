let page;
var homeindex = {
    homeindex_init: function () {
        homeindex_init();
    }
};
function homeindex_init() {
    page = 0;
    deletemonitorstation(0, TUDONG);//xoa ds trạm type =tudong
    GetStatoionDataFromTN(page); //lưu ds trạm type =tudong
    
}
