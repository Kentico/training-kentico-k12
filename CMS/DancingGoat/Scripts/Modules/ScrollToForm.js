define(['domReady!', 'jQuery'], function (document, $) {

    $('a[href^="#"]:not([href="#"])').click(function () {
        var target = $(this.hash);
        target = target.length ? target : $('[class=' + this.hash.slice(1) + ']');
        if (target.length) {
            $('html,body').animate({
                scrollTop: target.offset().top
            }, 1000);
        }
        return false;
    });
});