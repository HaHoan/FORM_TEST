$(function () {
    $('#submit1').click(function () {
        var answers = getAnswers();
        if (answers.length == $("textarea[name^='answer']").length) {
            $("#answers").val(JSON.stringify(answers))
            $('#submit-form').submit()
        }
        
    })
})
function getAnswers() {
    var answers = []
    $("textarea[name^='answer']").each(function (index, value) {
        if ($(this).val() == '') {
            alert("Bạn cần trả lời hết các câu hỏi!")
            return false;
        }
        var obj = {
            index: $('#question_' + (index + 1)).val(),
            value: $(this).val()
        }
        answers.push(obj)
    })
    return answers;
}