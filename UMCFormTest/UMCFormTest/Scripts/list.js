$(function () {
    $('#btn_add_question').click(function () {
        addQuestion($(this).attr('name'));
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


    $('.tableData').DataTable({
        initComplete: function () {
            this.api().columns(5).every(function () {
                var column = this;
                var select = $('<select class="form-control"><option value=""></option></select>')
                    .appendTo($(column.header()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );
                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });
                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });

            });
        },
        lengthMenu: [50, 100]
    })

})
function addQuestion(index) {
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
            deleteRow(index)
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
}
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
                    addQuestion(a)
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