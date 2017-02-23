$(function () {
    var counter = $('#counter');
    var date = new Date(counter.text());
    counter.countdown(
        {until: date});
    $(".sidebar__counter").css("display", "block");

    function serverTime() {
        var time = null;
        $.ajax({
            url: '/servertime.php',
            async: false, dataType: 'text',
            success: function (text) {
                time = new Date(text);
            }, error: function (http, message, exc) {
                time = new Date();
            }
        });
        return time;
    }
});