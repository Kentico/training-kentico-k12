define(['domReady!', 'jQuery'], function (domReady, $) {

    var $searchBox = $('.searchBox'),
        $menu = $('.menu'),
        $mobileMenuBtn = $('#mobile-menu-btn'),
        $mobileMenuIcon = $('#mobile-menu-icon'),
        $mobileMenuCaption = $('#mobile-menu-caption');

    menuSizeHandle();

    $(window).on('resize', function () {
        menuSizeHandle();
    });

    $mobileMenuBtn.on('click', toggleMenu);

    function menuSizeHandle() {
        if (window.matchMedia('(min-width: 768px)').matches) {
            showMenu();
            $menu.removeClass("mobile-menu");
            $menu.addClass("menu");
            $(".header").append($(".header-row"));
        }
        else if (!$menu.hasClass('mobile-menu')) {
            hideMenu();
            $menu.removeClass("menu");
            $menu.addClass("mobile-menu");
            $(".header").prepend($(".header-row")); 
        }
    }

    function toggleMenu() {
        if (menuIsVisible()) {
            hideMenu();
        } else {
            showMenu();
        }
    }

    function menuIsVisible() {
        return $menu.css('display') !== 'none';
    }

    function hideMenu() {
        $menu.hide();
        $searchBox.removeAttr('style');
        $mobileMenuIcon.removeClass('icon-modal-close');
        $mobileMenuIcon.addClass('icon-menu');
        $mobileMenuCaption.text('Open menu');
    }

    function showMenu() {
        $menu.show();
        $searchBox.css('display', 'inline-block');
        $mobileMenuIcon.removeClass('icon-menu');
        $mobileMenuIcon.addClass('icon-modal-close');
        $mobileMenuCaption.text('Close menu');
    }
});