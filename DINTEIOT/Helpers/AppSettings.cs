using System;

namespace WebApi.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
    }
    public class OptionFilter
    {
        public DateTime? startdate { get; set; }
        public int pagesize { get; set; } = 10;
        public int pagenumber { get; set; } = 1;
        public int pagefirst { get; set; } = 1;
        public string txtsearch { get; set; } = null;

        public string startdateafter
        {
            get
            {
                return startdate == null ? null : startdate.Value.ToString("yyyy-MM-dd h:mm tt");
            }
            set
            {
                //var startdatepre = startdate == null ? null : startdate.Value.ToString("yyyy-MM-dd h:mm tt");
                //Convert.ToDateTime(startdatepre);
            }
        }
        public DateTime? enddate { get; set; } = null;
        public string enddateafter
        {
            get
            {
                return enddate == null ? null : enddate.Value.ToString("yyyy-MM-dd h:mm tt");
            }
            set
            {
                //var enddatepre = enddate == null ? null : enddate.Value.ToString("yyyy-MM-dd h:mm tt");
                //Convert.ToDateTime(enddatepre);
            }
        }
    }
    public static class ScreenName
    {
        public const string StationData = "StationData";
        public const string Organ = "Organ";
        public const string WarningMargin = "WarningMargin";
        public const string MonitorStation = " MonitorStation";
        public const string StationDriver = " StationDriver";
        public const string StationMethod = " StationMethod";
        public const string Account = " Account";
        public const string MonitorDatabase = "MonitorDatabase";
        public const string ChartInfo = "ChartInfo";
    }
    public static class Vi_ScreenName
    {
        public static string INTRODUCE = "Liên Hệ";
        public static string FILEMANAGER = "Tư Liệu";
        public static string SENDQUESTION = "GỬI CÂU HỎI";
        public static string ORGAN = "organ";
        public static string LINKED = "linked";
        public static string INDUSTRY = "industry";
        public static string FM = "filemanager";
        public static string DOCUMENT = "Văn Bản";
        public static string ARTICLETYPE = "articletype";
        public static string ARTICLEPUBLISH = "articlepublish";
        public static string ARTICLEAPPROVE = "articlepublishappend";
        public static string LinkGroup = "linkgroup";
        public static string DocumentGroup = "documentgroup";
        public static string USER = "account";
        public static string HOME = "Trang chủ";
        public static string ARTICLE = "Tin tức";
        public static string SCHEDULE = "Lịch công tác";
    }
    public static class PositionConfig
    {
        public const string LINKEDSIDEBARRIGHT = "LK_SBR";//lấy ra các liên kết hiển thị ở phần sidebar phải
        public const string LINKEDBODY = "LK_BD"; //lấy ra các liên kết hiển thị ở phần body ,content
        public const string ARTICLEHIGHTLIGHT = "AC_HL"; //lấy ra các bài viết nổi bật
        public const string ARTICLETTSK = "TT_SK"; //lấy ra các bài viết thuộc chuyên mục tin tức sự kiện
        public const string ARTICLEEVENT = "AC_EV"; //lấy ra các bài viết thuộc chuyên mục sự kiện
        public const string ARTICLEACTIVITY = "AC_AV"; //lấy ra các bài viết thuộc chuyên mục hoạt động
        public const string ARTICLEDIRECTION = "AC_DR"; //lấy ra các bài viết thuộc chuyên mục chỉ đạo điều hành
        public const string ARTICLEENVIRONMENT = "AC_ENVIR"; //lấy ra các bài viết thuộc chuyên mục môi trường
        public const string ARTICLESEA = "AC_SEA"; //lấy ra các bài viết thuộc chuyên mục biển
        public const string ARTICLEWATER = "AC_WA"; //lấy ra các bài viết thuộc chuyên mục tài nguyên nước
        public const string ARTICLESOURCE = "AC_SC"; //lấy ra các bài viết thuộc chuyên mục địa chất khoáng sản
        public const string ARTICLELAND = "AC_LD"; //lấy ra các bài viết thuộc chuyên mục tài nguyên đất
        public const string ARTICLEMAP = "AC_MP"; //lấy ra các bài viết thuộc chuyên mục đo đạc và bản đồ
        public const string ARTICLEHYDRO = "AC_HYDRO"; //lấy ra các bài viết thuộc chuyên mục khí tượng thủy văn
        public const string ARTICLEVT = "AC_VT"; //lấy ra các bài viết thuộc chuyên mục viễn thám
        public const string ARTICLELIFE = "AC_LIFE"; //lấy ra các bài viết thuộc chuyên mục đời sống,kinh tế
        public const string ARTICLEAF = "AC_AF"; //lấy ra các bài viết thuộc chuyên mục thủ tục hành chính
        public const string ARTICLEIT = "AC_IT"; //lấy ra các bài viết thuộc chuyên mục công nghệ thông tin
        public const string ARTICLEIV = "AC_IV"; //lấy ra các bài viết thuộc chuyên mục đầu tư
        public const string ARTICLEST = "AC_ST"; //lấy ra các bài viết thuộc chuyên mục khoa học công nghệ
        public const string ARTICLEGT = "AC_GT"; //lấy ra các bài viết thuộc chuyên mục hợp tác quốc tế
        public const string ARTICLEDP = "AC_DP"; //lấy ra các bài viết thuộc chuyên mục hđ của địa phương trong tỉnh
        public const string ARTICLETT = "AC_TT"; //lấy ra các bài viết thuộc chuyên mục tin thanh tra
        public const string ARTICLEFT = "AC_FT"; //lấy ra các bài viết thuộc chuyên mục tin chuyên ngành
    }
    public static class RoleCode
    {
        public const string ADMIN = "ADMIN";  //mã nhóm quyền admin
        public const string LANHDAO = "ROLE_LD";  //mã nhóm quyền lãnh đạo
        public const string TRUONGPHONG = "ROLE_TP"; //mã nhóm quyền trưởng phòng
        public const string NHANVIEN = "ROLE_NV";  //mã nhóm quyền nhân viên
    }
    public static class Function
    {
        public const string ARTICLE = "QL_BV";
        public const string ARTICLECD = "QL_BVCD";
        public const string ARTICLECCB = "QL_BVCCB";
        public const string ARTICLECB = "QLBVCB";
        public const string ARTICLETYPE = "QL_LT";
        public const string ARTICLEBLOCK = "QL_KT";
        public const string ARTICLECATEGORY = "QL_CM";
        public const string DOCGROUP = "QL_NVB";
        public const string DOCTYPE = "QL_TLVB";
        public const string DOCUMENT = "QL_VB";
        public const string ARTICLEDRAFT = "QL_BVN";
        public const string AUTHORIZE = "QL_PQ";
        public const string LINKED = "QL_LK";
        public const string LINKEDGROUP = "QL_LKGR";
        public const string ORGAN = "QL_CQ";
    }
    public static class ArticleTypeCode
    {
        public const string TIN_VIDEO = "TINVIDEO";
        public const string TIN_ANH = "TINANH";
        public const string TIN_CHUA_TEP_DINH_KEM = "TINCOTEPDINHKEM";
    }
    public static class AuthorizeCode
    {
        public const string THEM = "Them";
        public const string SUA = "Sua";
        public const string XEM = "Xem";
        public const string XOA = "Xoa";
        public const string DUYET = "Duyet";
        public const string CONGBO = "CongBo";
    }
}


