$(function () {
    var $auth = $('.auth'),
        $popupWrp = $('.popup__wrp'),
        $popup  = $popupWrp.find('.popup'),
        $form = $('.popup__login-form'),
        popupOpened = false,
        POPUP_WIDTH = 262,
        POPUP_HEIGHT = 219;

    $auth.find('.auth__login').on('click', function () {
        showPopup();
    });

    $popupWrp.on('click', function (event) {
        if (!$(event.target).closest('.popup').length) {
            closePopup();
        }
    });

    $(document).on('keyup', function (event) {
        if (event.keyCode === 27 && popupOpened) {
            closePopup();
        }
    });

    function showPopup () {
        var offset = $auth.offset(),
            leftStart = offset.left,
            topStart = offset.top - $(document).scrollTop(),
            $window = $(window),
            authHeight, leftFinish, topFinish;

        popupOpened = true;

        $popupWrp.addClass('show');
        $auth.css('visibility', 'hidden');

        authHeight = $auth.height();

        $popup.css({
            left: leftStart + 'px',
            top: topStart + 'px',
            height: authHeight
        });

        leftFinish = ~~(($window.width() - POPUP_WIDTH) / 2);
        topFinish = ~~(($window.height() - POPUP_HEIGHT) / 2);

        $popup.stop().animate({
            left: leftFinish,
            top: topFinish,
            height: '170px'
        }, 600, function () {
            $popup.css('height', 'auto');
        });

        $form.fadeIn(3000);

        $popup.find('input').first().focus();
    }

    function closePopup () {
        $form.hide();
        $popupWrp.removeClass('show');
        $auth.css('visibility', 'visible');
        popupOpened = false;
    }
});

$(function () {
    var $form = $('.popup__login-form'),
        error;

    if (!$form.length) return;

    error = new errorConstructor($form.find('.popup__error'));

    $form.find('input').on('keyup', function() {
        error.hide();
    });

    $form.on('submit', function(e) {
        e.preventDefault();
        $.ajax({
            type: 'post',
            url: this.action,
            data: $form.serialize(),
            beforeSend: function() {
                error.hide();
            },
            success: function(data) {
                if (data.result === true) {
                    location.reload();
                } else if (data.error) {
                    error.show(data.error);
                } else {
                    error.show('Непредвиденная ошибка');
                }
            },
            error: function(xhr, desc, err) {
                error.show('[' + desc + '] ' + err);
            }
        });
    });

    function errorConstructor($el) {
        this.$el = $el;
        this.show = function(msg) {
            this.$el.html(msg).show();
        };
        this.hide = function() {
            this.$el.hide().empty();
        };
    }
});