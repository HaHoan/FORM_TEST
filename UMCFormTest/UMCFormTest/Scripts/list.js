$(function () {
    $('#table-content').DataTable();
    $('#btn_add_question').click(function () {

        var number = $(".list_question label").length;
        number = number + 1;
        var div = $('<div />', {
            id: 'form_' + number
        })
        $('.list_question').append(div)
        var label = $('<label />', {
            class: "lang font-weight-bold mt-2",
            key: "question",
            id: 'label_' + number

        })
        var span = $('<span />', {
            class: 'font-weight-bold ml-1',
            text: number,
            id: "question_" + number
        })

        var btnXoa = $('<button/>', {
            type: 'button',
            text: 'x',
            class: 'btn-delete text-danger font-weight-bold ml-1',
            click: function (e) {
                deleteRow($(this).attr('name'))
            },
            name: number
        });
        div.append(label)
        div.append(span)
        div.append(btnXoa)
        var inputVi = $('<input />', {
            class: 'form-control mt-1',
            placeholder: "Tiếng Việt",
            id: "question_" + number + "_vi"
        })

        var inputJa = $('<input />', {
            class: 'form-control mt-1',
            placeholder: "Tiếng Nhật",
            id: "question_" + number + "_ja"
        })

        div.append(inputVi)
        div.append(inputJa)

        var lang = localStorage.getItem('lang') || 'en';
        changeLanguage(lang);
    })
    var number = $(".list_question div").length;
    for (var i = 1; i <= number; i++) {
        $('#delete_' + i).click(function (e) {
            deleteRow($(this).attr('name'))
        })

    }
    $('#add_submit').click(function () {
       
        if ($("[name^='name_exam']").val() == '') {
            alert("Bạn cần nhập tên tiêu đề bài thi")
            return false;
        }
    
        var question = getQuestions();
        if (question.length == 0) {
            alert("Bạn cần nhập các câu hỏi")
            return false;
        }
        if ($("[name='target']").val() == '') {
            alert("Bạn cần nhập target của bài thi")
            return false;
        }
        $("#list_question").val(JSON.stringify(question))
        $('#add_form').submit()
    })

})
function deleteRow(index) {
    $('#form_' + index).remove();
}

function getQuestions() {
    var list = [];
    $('.list_question div').each(function (index, value) {
        var i = value.id.substring(5, value.id.length)
        var questionVi = $('#question_' + i + "_vi").val()
        var questionJa = $('#question_' + i + "_ja").val()
        if (questionVi != '' && questionJa != '') {
            var obj = {
                index: index,
                vi: questionVi,
                ja: questionJa
            }
            list.push(obj)
        }

    })
    return list;
}