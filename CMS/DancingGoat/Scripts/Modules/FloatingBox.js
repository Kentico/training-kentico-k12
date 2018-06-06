// Handles floating of element within a page. Floating style of element must be defined in 'fixed' class.
//

define(['domReady!', 'jQuery'], function (document, $) {

    // Remove floating styles for mobile device
    var checkMobile = function($box) {
        if (window.matchMedia('(max-width: 767px)').matches) {
            $box.removeClass('fixed');
            $box.width("");
            return true;
        }
        return false;
    };

    var updatePosition = function($box, initialTop) {
        var scrollTop = $(window).scrollTop(),
            boxHeight = $box.outerHeight(),
            footerTop = $('.footer-container').offset().top,
            bottomOffset = parseFloat($('html').css('font-size'));
        
        // Skip mobile device
        if (checkMobile($box)) {
            return;
        }

        if (scrollTop >= initialTop) {
            // Add class with fixed position and ensure right width
            $box.addClass('fixed');
            $box.outerWidth($box.parent().width());

        } else {
            // Remove fixed class
            $box.removeClass('fixed');
            $box.width("");
        }

        // Stop floating over footer
        if (footerTop < (scrollTop + boxHeight + bottomOffset)) {
            $box.css("top", (footerTop - boxHeight - scrollTop - bottomOffset) + "px");
        } else {
            $box.css("top", "");
        }
    };

    // Add floating functionality 
    $('.floating-box').each(function (elementPosition, box) {
        var $box = $(box),
            boxMargin = parseFloat($box.css('marginTop').replace(/auto/, 0)),
            initialTop = $box.offset().top - boxMargin;

        // Update style according scroll position
        $(window).on("scroll", function () {
            updatePosition($box, initialTop);
        });

        // Ensure right styles when window is resized
        $(window).on("resize", function () {

            // Ensure width if box is floating
            if ($box.hasClass('fixed')) {
                $box.outerWidth($box.parent().width());
            }

            if (!checkMobile($box)) {
                // Ensure right position
                if (!$box.hasClass('fixed')) {
                    initialTop = $box.offset().top - boxMargin;
                }
                updatePosition($box, initialTop);
            }
        });
        
        // Ensure right position after postback with scrolled page
        updatePosition($box, initialTop);
    });
});