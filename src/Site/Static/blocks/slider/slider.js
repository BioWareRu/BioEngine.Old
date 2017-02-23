$(function () {
    var $carouselWrap = $('.slider'),
        $carousel = $carouselWrap.find('.slider__list-items'),
        $item = $carousel.find('.slider__list-item').eq(0),
        carouselWrapPadding = $carouselWrap.outerWidth() - $carousel.innerWidth(),
        itemWidth = $item.width(),
        itemsMinNumber = 6,
        itemsMaxNumber = 12,
        responsive = { 0: { items: itemsMinNumber } },
        breakpoint, i, steps;

    for (i = 1, steps = itemsMaxNumber - itemsMinNumber; i < steps; i++) {
        breakpoint = itemWidth * (itemsMinNumber + i) + carouselWrapPadding;
        responsive[breakpoint] = { items: itemsMinNumber + i };
    }

    $carousel.owlCarousel({
        loop: true,
        dots: false,
        responsive: responsive
    });

    $carousel.on('mousewheel', '.owl-stage', function (e) {
        if (e.deltaY > 0) {
            $carousel.trigger('next.owl.carousel');
        } else {
            $carousel.trigger('prev.owl.carousel');
        }
        e.preventDefault();
    });

    $carouselWrap.find('.slider__control_left').on('click', function() {
        $carousel.trigger('prev.owl.carousel');
    });
    $carouselWrap.find('.slider__control_right').on('click', function() {
        $carousel.trigger('next.owl.carousel');
    });
});
