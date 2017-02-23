$(function () {
    jQuery("#nanoGallery").nanoGallery({
        thumbnailWidth: 'auto',
        thumbnailHeight: 140,
        locationHash: true,
        thumbnailLabel: {display: false},
        thumbnailGutterWidth: 10,
        thumbnailGutterHeight: 10,
        thumbnailHoverEffect: 'imageScaleIn80',
        touchAnimation: false,
        viewerToolbar: {
            style: 'stuckImage'
        }
    });
});