$(function () {
    /* Mutil Language */
    var lang = localStorage.getItem('lang') || 'en';
    changeLanguage(lang);

})
function changeLanguage(lang) {
    if (lang == "en") {
        $(".icon-lang").attr('src', "/Resource/united-states.png");
        $(".text-lang").text("English")
    } else if (lang == "vi") {
        $(".icon-lang").attr('src', "/Resource/vietnam.png");
        $(".text-lang").text("Tiếng Việt")
    } else if (lang == "ja") {
        $(".icon-lang").attr('src', "/Resource/japan.png");
        $(".text-lang").text("日本語")
    }
    $(".lang").each(function (index, element) {
        $(this).text(arrLang[lang][$(this).attr("key")]);
        $(this).attr("placeholder", arrLang[lang][$(this).attr("key")]);
    });
}
$(".translate").click(function () {
    var lang = $(this).attr("id");
    localStorage.setItem('lang', lang);
    changeLanguage(lang);

});
var arrLang = {
    "en": {
        "name": "Full Name",
        "code": "Staff Code",
        "dept": "Department",
        "mark": "Mark     :",
        "review": "Review:",
        "date": "Date",
        "title": "Bài kiểm tra:",
        "submit": "Submit",
        "logout": "Log out",
        "login": "Log in",
        "pass": "Password",
        "code": "StaffCode:",
        "success": "You submitted successfully!",
        "error": "Error",
        "reviewer": "Mark the test",
        "add_question": "Add New Exam",
        "test": "Exam",
        "done": "Done",
        "point": "Point",
        "answer_question":"Do exam"
    },
    "vi": {
        "name": "Họ và tên",
        "code": "Mã nhân viên",
        "dept": "Bộ phận",
        "mark": "Điểm đạt :",
        "review": "Đánh giá",
        "date": "Ngày Thi:",
        "title": "Bài kiểm tra:",
        "submit": "Nộp",
        "logout": "Đăng xuất",
        "login": "Đăng nhập",
        "pass": "Mật khẩu",
        "success": "Bạn đã nộp thành công!",
        "error": "Có lỗi sảy ra",
        "reviewer": "Chấm điểm bài thi",
        "add_question": "Thêm bài thi mới",
        "test": "Bài thi",
        "done": "Xong",
        "point": "Điểm",
        "answer_question": "Làm bài thi"
    },
    "ja": {
        "name": "名前   :",
        "code": "社員コードー:",
        "dept": "部署:",
        "mark": "合格点数: ",
        "review": "評価:",
        "date": "テスト日: ",
        "title": "",
        "submit": "Submit",
        "logout": "ログアウト",
        "login": "ログイン",
        "pass": "パスワード",
        "code": "社員コードー",
        "success": "You submitted successfully!",
        "error": "Error",
        "reviewer": "Mark the test",
        "add_question": "Add New Exam",
        "test": "Exam",
        "done": "Done",
        "point": "Point",
        "answer_question": "Do exam"
    }
};
