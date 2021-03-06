$(function () {
    /* Mutil Language */
    var lang = localStorage.getItem('lang') || 'vi';
    changeLanguage(lang);

})
function changeLanguage(lang) {
    if (lang == "en") {
        $(".icon-lang").attr('src', "/Resource/united-states.png");
        $(".text-lang").text("English")
    } else if (lang == "vi") {
        $(".icon-lang").attr('src', "/Resource/vietnam.png");
        $(".text-lang").text("Tiếng Việt")

        $('[id^=' + 'question_vi_' + ']').removeClass("d-none")
        $('[id^=' + 'question_ja_' + ']').addClass("d-none")
        $('[id^=' + 'name_vi' + ']').removeClass("d-none")
        $('[id^=' + 'name_ja' + ']').addClass("d-none")
        $('[name = "form_multiple_choice_vi"]').removeClass('d-none')
        $('[name = "form_multiple_choice_ja"]').addClass('d-none')
    } else if (lang == "ja") {
        $(".icon-lang").attr('src', "/Resource/japan.png");
        $(".text-lang").text("日本語")

        $('[id^=' + 'question_vi_' + ']').addClass("d-none")
        $('[id^=' + 'question_ja_' + ']').removeClass("d-none")

        $('[id^=' + 'name_vi' + ']').addClass("d-none")
        $('[id^=' + 'name_ja' + ']').removeClass("d-none")

        $('[name = "form_multiple_choice_vi"]').addClass('d-none')
        $('[name = "form_multiple_choice_ja"]').removeClass('d-none')

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
        "is_correct_answer":"Is Correct Answer?",
        "answer_question": "Do exam",
        "name_exam": "Name",
        "date_create": "Date Create",
        "add_new_exam": "Create New Exam",
        "close": "Close",
        "save": "Save",
        "question": "Question",
        "type_question": "Question Type",
        "multiple_choice_answer":"Option",
        "isCurrent": "Is Current",
        "target": "Target",
        "register": "Register",
        "new_member": "Register New Member",
        "newpass": "New Password",
        "repass": "Reenter Password",
        "is_reviewer": "Is Reviewer",
        "change_password": "Change Password",
        "old_password": "Old Password",
        "confirm_submit": "Do you want to submit now?",
        "cancel": "Cancel",
        "delete": "Delete",
        "confirm_delete": "Do you want to delete now?",
        "confirm_delete_exam_doing": "This exam has have a people doing. Do you want to delete all? "
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
        "done": "Chấm điểm",
        "point": "Điểm",
        "answer_question": "Làm bài thi",
        "name_exam": "Tiêu đề bài thi",
        "date_create": "Ngày tạo",
        "add_new_exam": "Tạo bài thi mới",
        "close": "Đóng",
        "save": "Lưu thay đổi",
        "question": "Câu hỏi",
        "type_question": "Loại câu hỏi",
        "multiple_choice_answer": "Đáp án",
        "isCurrent": "Chọn làm bài thi hiện tại",
        "target": "Điểm tối đa",
        "register": "Đăng ký",
        "new_member": "Đăng ký thành viên mới",
        "newpass": "Mật khẩu",
        "repass": "Gõ lại mật khẩu",
        "is_reviewer": "Là người đánh giá",
        "change_password": "Đổi Mật Khẩu",
        "old_password": "Mật khẩu cũ",
        "confirm_submit": "Bạn có muốn nộp ngay không?",
        "cancel": "Hủy",
        "delete": "Xóa",
        "confirm_delete": "Bạn có muốn xóa bài kiểm tra này không?",
        "confirm_delete_exam_doing": "Bài thi này đã có người làm! Bạn có muốn xóa hết các bài đã làm không?",
        "is_correct_answer": "Có phải là đáp án đúng không?"
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
        "done": "Submit",
        "point": "Point",
        "answer_question": "Do exam",
        "name_exam": "Name",
        "date_create": "Date Create",
        "add_new_exam": "Create New Exam",
        "close": "Close",
        "save": "Save",
        "question": "問題",
        "type_question": "Question Type",
        "multiple_choice_answer": "Option",
        "isCurrent": "Is Current",
        "target": "Target",
        "register": "登録",
        "new_member": "新 メンバー",
        "newpass": "New Password",
        "repass": "Reenter Password",
        "is_reviewer": "Is Reviewer",
        "change_password": "Change Password",
        "old_password": "Old Password",
        "confirm_submit": "Do you want to submit now?",
        "confirm_delete": "Do you want to delete now?",
        "cancel": "Cancel",
        "delete": "Delete",
        "confirm_delete_exam_doing": "This exam has have a people doing. Do you want to delete all? ",
        "is_correct_answer": "Is Correct Answer?"
    }
};
