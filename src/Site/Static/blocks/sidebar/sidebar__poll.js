$(function () {
    $('.sidebar__poll-input').on('change', function () {
        var form = $(this).parents('.sidebar__poll');
        if ($('.sidebar__poll-input:checked', form).length) {
            $('.sidebar__poll-submit', form).removeAttr('disabled');
        } else {
            $('.sidebar__poll-submit', form).attr('disabled', 'disabled');
        }
    });
    $('.sidebar__poll-submit').on('click', function (e) {
        e.preventDefault();
        var form = $(this).parents('.sidebar__poll form');
        console.log('get fingerprint');
        getUserFingerPrint(function (fingerprint) {
            console.log('fingerprint', fingerprint);
            $('input[name=fingerprint]', form).val(fingerprint);
            console.log('submit');
            form.submit();
        });
    });
});