$(function () {
    $('#btn_add_question').click(function () {
        addQuestion();
        var lang = localStorage.getItem('lang') || 'en';
        changeLanguage(lang);
    })
    var number = $(".list_question div").length;
    for (var i = 1; i <= number; i++) {
        $('#question_delete_' + i).click(function (e) {
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
    $('#btn_delete_exam').click(function () {
        IsExamDoing($("[name='id_exam'").val());
    })


    $("[name^='name_title_'").click(function () {
        var name = $(this).attr('name');
        var indexOf = name.lastIndexOf('_');
        var id = name.substr(indexOf + 1, name.length - 1)
        getExamDetail(id)
    })


    $('[id^="type_question_"]').change(function () {
        var id = $(this).attr('id')
        var no = $(this).attr('name')

        var type_question = $('#' + id + ' option:selected').val()

        if (type_question == 'multiple_choice') {
            $('#add_answer_' + no).removeClass('d-none')
            $('#list_multiple_choice_' + no).removeClass('d-none')
        } else {
            $('#add_answer_' + no).addClass('d-none')
            $('#list_multiple_choice_' + no).addClass('d-none')
        }
    })
    $('[id^="add_answer_"]').click(function (e) {
        var no = $(this).attr('name')
        var number = $("#list_multiple_choice_" + no + " .multiple_choice").length;

        addAnswerMultiChoice(no, number + 1)
    })
})
function deleteAnswer(context) {
    $(context).parents('.multiple_choice').remove();
}
function addAnswerMultiChoice(no, index) {
    var multiple_choice = $('<div />', {
        class: 'multiple_choice mb-3'
    })
    var label = $('<label />', {
        class: 'lang font-italic mt-2',
        key: 'multiple_choice_answer',
        text: 'Đáp án'
    })
    var span = $('<span />', {
        class: 'font-italic',
        id: 'answer_no_' + no + '_' + index,
        text: ' ' + index
    })
    var btn_delete = $('<button />', {
        class: 'btn-delete font-weight-bold',
        id: 'answer_delete_' + no + "_" + index,
        name: no + '_' + index,
        text: 'x',
        click: function (event) {
            event.preventDefault();
            $(this).parents('.multiple_choice').remove();
        }
    })
    var textVi = $('<textarea />', {
        class: 'form-control mt-1',
        placeholder: "Tiếng Việt",
        id: 'answer_' + no + '_' + index + '_vi'
    })
    var textJp = $('<textarea />', {
        class: 'form-control mt-1',
        placeholder: "Tiếng Nhật",
        id: 'answer_' + no + '_' + index + '_ja'
    })

    var divCheck = $('<div />', {
        class: 'form-check'
    })

    var inputCheck = $('<input />', {
        class: 'form-check-input',
        type: 'checkbox',
        id: 'correct_answer_' + no + '_' + index
    })
    var labelCheck = $('<label />', {
        class: 'form-check-label lang',
        key: 'is_correct_answer',
        text: 'Chọn làm đáp án đúng'
    })
    divCheck.append(inputCheck)
    divCheck.append(labelCheck)

    multiple_choice.append(label)
    multiple_choice.append(span)
    multiple_choice.append(btn_delete)
    multiple_choice.append(textVi)
    multiple_choice.append(textJp)
    multiple_choice.append(divCheck)
    $('#list_multiple_choice_' + no).append(multiple_choice)
}
function addQuestion() {
    var number = $(".list_question .row").length;
    number = number + 1;
    var bg = 'bg-light'
    if (number % 2 == 0) {
        bg = 'bg-white'
    }
    var div = $('<div />', {
        id: 'form_' + number,
        class: 'row  mt-5 ' + bg
    })
    
    var div1 = $('<div />', {
        class: 'col'
    })

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
    div1.append(label)
    div1.append(span)
    div1.append(btnXoa)
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

    div1.append(inputVi)
    div1.append(inputJa)

    var list_multi = $('<div />', {
        id: "list_multiple_choice_" + number
    })
    var add_answer = $('<a />', {
        href: "javascript:void(0)",
        class: 'd-none',
        id: 'add_answer_' + number,
        name: number,
        text: 'Thêm đáp án',
        click: function () {
            var no = $(this).attr('name')
            var number = $("#list_multiple_choice_" + no + " .multiple_choice").length;
            addAnswerMultiChoice(no, number + 1)
        }
    })
    div1.append(list_multi)
    div1.append(add_answer)
    div.append(div1)

    var div2 = $('<div />', {
        class: 'col-2'
    })

    var label2 = $('<label />', {
        class: 'lang font-weight-bold mt-2',
        key: 'type_question'
    })
    var select = $('<select />', {
        class: 'form-control mt-1',
        id: 'type_question_' + number,
        name: number,
        change: function () {
            var id = $(this).attr('id')
            var no = $(this).attr('name')

            var type_question = $('#' + id + ' option:selected').val()

            if (type_question == 'multiple_choice') {
                $('#add_answer_' + no).removeClass('d-none')
                $('#list_multiple_choice_' + no).removeClass('d-none')
            } else {
                $('#add_answer_' + no).addClass('d-none')
                $('#list_multiple_choice_' + no).addClass('d-none')
            }
        }
    })
    var option1 = $('<option />', {
        value: 'text',
        text: "Tự luận"
    })
    var option2 = $('<option />', {
        value: 'multiple_choice',
        text: "Trắc nghiệm"
    })
    select.append(option1)
    select.append(option2)
    div2.append(label2)
    div2.append(select)
    div.append(div2)
   
    $('.list_question').append(div)
}
function deleteRow(index) {
    $('#form_' + index).remove();
}
function getQuestions() {
    var list = [];
    $('.list_question .row').each(function (index, value) {
        var i = value.id.substring(5, value.id.length)
        var questionVi = $('#question_' + i + "_vi").val()
        var questionJa = $('#question_' + i + "_ja").val()
        if (questionVi != '' && questionJa != '') {
            var type_question = $('#type_question_' + i + ' option:selected').val()
            if (type_question == 'multiple_choice') {
                var list_answer = []
                $('#list_multiple_choice_' + i + ' .multiple_choice').each(function (ai, av) {
                    var answerVi = $('#answer_' + i + '_' + (ai + 1) + '_vi').val()
                    var answerJa = $('#answer_' + i + '_' + (ai + 1) + '_ja').val()
                    var isCorrect = $('#correct_answer_' + i + '_' + (ai + 1)).is(':checked')
                    var answer = {
                        index: (ai + 1),
                        vi: answerVi,
                        ja: answerJa,
                        isCorrect: isCorrect
                    }
                    list_answer.push(answer)
                })
                var obj = {
                    index: index,
                    vi: questionVi,
                    ja: questionJa,
                    type_question: type_question,
                    list_answer: list_answer
                }

                list.push(obj)
            } else {
                var obj = {
                    index: index,
                    vi: questionVi,
                    ja: questionJa,
                    type_question: type_question
                }

                list.push(obj)
            }
           
        }

    })
    return list;
}
function getExamDetail(Id) {
    $.ajax({
        url: "/Home/GetExamDetail",
        type: "GET",
        data: {
            Id: Id
        },
        success: function (response) {
            const exam = JSON.parse(response.exam.Name);
            $("[name^='name_exam_vi']").val(exam.vi)
            $("[name^='name_exam_ja']").val(exam.ja)
            $("[id^='question_']").val("")
            var length = response.question.length
            var currentRow = $('[id^="form_"]').length;
            if (currentRow < length) {
                for (var a = currentRow; a < length; a++) {
                    addQuestion()
                }
            }
            $.each(response.question, function (i, value) {
                const ques = JSON.parse(value.ques);
                $("#question_" + (i + 1) + "_vi").val(ques.vi);
                $("#question_" + (i + 1) + "_ja").val(ques.ja);
            });
            $("[name='target'").val(response.exam.Target)
            if (response.exam.IsCurrent == true) {
                $('#isCurrent').prop('checked', true);
            } else {
                $('#isCurrent').prop('checked', false);
            }
            $("[name='id_exam'").val(response.exam.ID);
            $("#btn_delete_exam").removeClass('d-none')
            if (response.isEdit == false) {
                $("[name^='name_exam_vi']").prop("readonly", true);
                $("[name^='name_exam_ja']").prop("readonly", true);
                $("[id^='question_']").prop("readonly", true);
                $("[name='target'").prop("readonly", true);
                $("[id^='question_delete_'").addClass('d-none')
                $("[id='btn_add_question'").addClass('d-none')

            } else {
                $("[name^='name_exam_vi']").prop("readonly", false);
                $("[name^='name_exam_ja']").prop("readonly", false);
                $("[id^='question_']").prop("readonly", false);
                $("[name='target'").prop("readonly", false);
                $("[id^='question_delete_'").removeClass('d-none')

            }
        },
        error: function (e) {

        }
    });
}
function IsExamDoing(Id) {
    $.ajax({
        url: "/Home/IsExamDoing",
        type: "GET",
        data: {
            Id: Id
        },
        success: function (response) {
            if (response.result == 'IS_DOING') {
                $('#confirmDeleteExamDoingModal').modal('show');
            } else {
                $('#confirmDeleteExam').modal('show');
            }
        },
        error: function (e) {
            alert("Có lỗi xảy ra!")
        }
    });
}