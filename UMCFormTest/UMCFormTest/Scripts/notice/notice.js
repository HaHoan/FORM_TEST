$(function () {
    var noticeHub = $.connection.notice;
    loadClient(noticeHub);
    $.connection.hub.start().done(function () {
        noticeHub.server.connect("userChat");
    });


})
function Send(name, msg) {
    var chatHub = $.connection.notice;
    var split = name.split(",");
    $.each(split, function (index, value) {
        if (value != "")
            chatHub.server.message(value, msg);
    });

}
function loadAllNotice(name, msg) {
    var item = $('<p />', {
        text: name + msg,
        class: 'dropdown-item'
    })
    var div = $('<div />', {
        class: 'dropdown-divider'
    })
    $('#list_notice').append(item)
    $('#list_notice').append(div)
    var numberNotice = $('#list_notice .dropdown-item').length
    if (numberNotice > 0) {
        $('#number_notice').text(numberNotice)
    }

}
function loadClient(chatHub) {
    chatHub.client.message = function (name, msg) {
        loadAllNotice(name, msg);
    }
}
