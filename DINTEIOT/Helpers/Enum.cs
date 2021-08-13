using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DINTEIOT.Helpers
{
    public enum Enum
    {
        Draft = 0, // bản nháp
        ApproveAppend = 1, //xuất bản
        Approve = 2, // đã duyệt
        //PublishAppend = 3,
        Publish = 3, // đã công bố
        Unpublish = 4  // hủy công bố
    }
    public enum FileType
    {
        avatar = 0, // ảnh đại diện
        imagesilde = 1, // sile ảnh
        video = 2,// video,
        document = 3,  // file văn bản
        linked = 4,  // ảnh đại diện của liên kết,
        filemanager = 5, // file tư liệu
        adminformality = 6 , //file thủ tục hành chính,
        answerfile = 7,  //file trả lời câu hỏi (nếu có)
        articlemediafile = 8  //file trả lời câu hỏi (nếu có)

    }
    public enum CapQuanLyEnum
    {
        CapTinh = 1, // cấp tỉnh
        CapHuyen = 2, // cấp huyện
        CapXa = 3,// cấp xã,
        CapSo = 4, // cấp sở
        CacDonViKhac = 5
    }
    public enum QAEnum
    {
        CauHoiMoi = 0,
        CauHoiDaDuocTiepNhan = 1
        //CauHoiDaDuocTraLoi = 5,
        //CauHoiDaDuocTraLoiVaDuocDuyet = 3,
        //CauHoiVaTraLoiDaDuocCongBo = 4,
        //CauHoiVaTraLoiDaHuyCongBo = 5,
    }
    public enum AnswerEnum
    {
        CauTraLoiMoi = 1,//câu hỏi đã có câu trả lời
        CauTraLoiGuiDuyet = 2, // câu hỏi đã có câu trả  lời và đã được gửi duyệt
        CauTraLoiDuocDuyet =3,// câu hỏi đã có câu trả  lời và đã được  duyệt
        CauTraLoiHuyDuyet = 4, // câu hỏi đã có câu trả  lời và đã hủy  duyệt
        CauTraLoiDuocCongBo = 5, // câu hỏi đã có câu trả  lời và đã công bố
        CauTraLoiHuyCongBo =6 // hủy công bố
    }
    public enum Interaction //tương tác
    {
        View = 0,
        Comment =1,
        Rate =2
    }
    public enum ScheduleStatus //lịch công tác status
    {
        Nhap = 0,
        CongBo = 1,
        HuyLich = 2,
        HuyCongBo =3
    }
    public enum Role // nhóm quyền người dùng
    {
        Admin = 1,
        LanhDao = 2,
        TruongPhong = 3,
        NhanVien = 4
    }
    public enum Permission // nhóm quyền người dùng
    {
        Them = 1,
        Sua = 2,
        Xoa = 3,
        Xem = 4
    }
    public enum DocStatusEnum // nhóm quyền người dùng
    {
       CongBo =1,
       HuyCongBo =2
    }
    public enum ExitCodes
    {
        NotAuthorize =-1,
        Success = 1,
        Error = 0,
        SignToolNotInPath = 2,
        AssemblyDirectoryBad = 3,
        PFXFilePathBad = 4,
        PasswordMissing = 8,
        SignFailed = 16,
        UnknownError = 32
    }
}
