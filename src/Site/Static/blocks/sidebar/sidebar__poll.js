$(function () {
    $('.sidebar__poll-input').on('change', function () {
        var form = $(this).parents('.sidebar__poll');
        if ($('.sidebar__poll-input:checked', form).length) {
            $('.sidebar__poll-submit', form).removeAttr('disabled');
        } else {
            $('.sidebar__poll-submit', form).attr('disabled', 'disabled');
        }
    });
});