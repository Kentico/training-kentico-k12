define(['domReady!', 'jQuery'], function (document, $) {
    $('.lp-section-background').each(function () {
        var $img = $(this).find('img');
        $(this).css('background-image', 'url(' + $img.attr("src") + ')');
        $img.remove();
    });
});