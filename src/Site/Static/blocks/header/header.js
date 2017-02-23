$(function () {
   $('.header__menu-item').on('click', function () {
       if ($('.header__submenu', this).length && !$(this).hasClass('header__menu-item_open')) {
           closeSubmenu();
           openSubmenu(this);
           return false;
       }
   });
    $(document).on('click', function(event) {
        if ($(event.target).closest('.header__submenu').length == 0 ) {
            closeSubmenu();
        }
    })
});

function openSubmenu (activeItem) {
    $(activeItem).addClass('header__menu-item_open');
    $('.header__submenu', activeItem).slideDown(300);
}

function closeSubmenu () {
    $('.header__menu-item_open').removeClass('header__menu-item_open');
    $('.header__submenu').slideUp(300);
}