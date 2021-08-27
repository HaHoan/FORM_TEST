$(function () {
   
    $('#submit').click(function () {
        var answers = getAnswers();
        if (answers.length == $("textarea[name^='answer']").length) {
            $("#answers").val(JSON.stringify(answers))
            $('#submit-form').submit()
        }

    })
    $('#point').keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            return false;
        }
    });
    $(".type_number").keypress(function (e) {
        return onlyNumber(e)
    });
    
})

function getAnswers() {
    var answers = []
    $("textarea[name^='answer']").each(function (index, value) {
        var obj = {
            index: $('#question_' + (index + 1)).val(),
            value: $(this).val()
        }
        answers.push(obj)
    })
    return answers;
}
function onlyNumber(e) {
    if (/\d+|,+|[/b]+|-+/i.test(e.key)) {
        return true
    } else {
        return false;
    }
}
