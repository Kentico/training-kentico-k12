jQuery(".carousel2d .jCarouselLite").jCarouselLite({
    'btnNext': ".ppright",
    'btnPrev': ".ppleft",
    'width': 50,
    'height': 50,
    'circular': false,
    'visible': 4
});
jQuery(document).ready(function () {
    jQuery("a.fancyboxProductImg").attr('rel', 'gallery').fancybox({
        'opacity': true,
        'transitionIn': 'elastic',
        'transitionOut': 'none',
        'padding': 20,
        'type': 'image',
        'centerOnScroll': true,
        'showNavArrows': true,
        'titleShow': true,
        'titlePosition': 'inside',
        'titleFormat': function (title, currentArray, currentIndex, currentOpts) {
            return '<div id="fancybox-title-over"><span class="title">' + (title.length ? ' &nbsp; ' + title : '') + '</span><span class="imageCount">Image ' + (currentIndex + 1) + ' / ' + currentArray.length + '</span></div>'
        }
    });
    jQuery(".carousel2d.main").show();
});