$(function () {
   
    $('#submit').click(function () {
        var answers = getAnswers();
        if (answers.length == $("textarea[name^='answer']").length) {
            $("#answers").val(JSON.stringify(answers))
            Send($('#full_name').text(), " Vừa nộp bài " + $('#title_exam').val())
            $('#submit-form').submit()
        }

    })
    
    $('#reviewBtn').click(function () {
     
        var answers = getAnswers();
      
        if (answers.length == $("textarea[name^='answer']").length) {
            $("#answers").val(JSON.stringify(answers))
            $('#review-form').submit()
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
    $('input[id^="multiple_choice_"').click(function (e) {
        var id = $(this).attr('id')
        var lastIndex = id.lastIndexOf('_')
        var no = id.substr(lastIndex + 1, id.length - lastIndex - 1)
        var answer = $("#" + id + ":checked").val();
        if (answer == 'on') {
            $('textarea[name="answer_' + $(this).attr('name') + '"]').val(no)
        }
    });
})

function getAnswers() {
    var answers = []
  
    $("textarea[name^='answer']").each(function (index, value) {
        var question = $('#question_' + (index + 1)).val();
        if (question == null) question = ""

        var value = $(this).val()
        if (value == null) value = ""

        var point = $('#point_' + (index + 1)).val()
        if (point == null) point = ""

        var comment = $('#comment_' + (index + 1)).val()
        if (comment == null) comment = ""
        var type_question = $('#type_question_' + (index + 1)).val()
        if (type_question == 'text') {

        }
        var obj = {
            index: question,
            value: value,
            point: point,
            comment: comment 
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
