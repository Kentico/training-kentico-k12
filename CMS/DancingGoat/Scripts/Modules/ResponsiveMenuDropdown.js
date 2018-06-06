define(['domReady!', 'jQuery'], function (domReady, $) {
    
    // Open or close dropdown
    var toogleDropdown = function($dropdownHeader, $dropdownMenu) {

        if (isTablet()) {
            handleTabletDropdownPosition($dropdownHeader, $dropdownMenu);
        }
        else if (isDesktop()) {
            ensureDesktopDropdownWidth($dropdownHeader, $dropdownMenu);
        }

        if (!isMobile() || $dropdownHeader.hasClass('dropdown-header-mobile')) {
            $dropdownMenu.slideToggle("fast");
            $dropdownHeader.toggleClass('dropdown-opened');
            $dropdownHeader.find('.dropdown-arrow').toggleClass("icon-chevron-down icon-chevron-up");
        }
    };
    
    // Handle window width change
    var handleResponsivness = function($dropdownHeader, $dropdownMenu) {       
        if (isDesktop()) {
            ensureDesktopDropdownWidth($dropdownHeader, $dropdownMenu);

            if ($dropdownMenu.hasClass('dropdown-desktop-visible')) {
                $dropdownMenu.removeAttr('style');
                $dropdownHeader.removeClass('dropdown-opened');
            }
            return;
        }
        
        $dropdownMenu.css('min-width', '');
        $dropdownMenu.css('margin-left', '');
          
        if (isMobile() && !$dropdownHeader.hasClass('dropdown-header-mobile')) {
            $dropdownHeader.removeClass('dropdown-opened');
            $dropdownMenu.removeAttr('style');
            $dropdownHeader.find('.dropdown-arrow').removeClass('icon-chevron-up').addClass('icon-chevron-down');
            return;
        }
        
        if (isTablet()) {
            handleTabletDropdownPosition($dropdownHeader, $dropdownMenu);
        }        
    };

    // Close dropdown if user clicked on something else
    var handleGlobalClick = function(e) {
        $('.dropdown-header').each(function (elementPosition, header) {
            var $dropdownHeader = $(header);
            var $dropdownMenu = $(header).next('nav').children('.dropdown-items-list');
            
            if ($dropdownHeader.hasClass("dropdown-opened") &&
                !$dropdownMenu.is(e.target) &&
                !$dropdownMenu.has(e.target).length &&
                !$dropdownHeader.is(e.target) &&
                !$dropdownHeader.has(e.target).length) {
                toogleDropdown($dropdownHeader, $dropdownMenu);
            }
        });
    };
    
    // Change dropdown direction to left if window is too small
    var handleTabletDropdownPosition= function($dropdownHeader, $dropdownMenu) { 
        var visible = $dropdownHeader.hasClass("dropdown-opened");       
        if (!visible) {
            $dropdownMenu.show();
        }                                                                      
        if ($dropdownMenu.offset().left + $dropdownMenu.outerWidth() > $(window).width()) {
            $dropdownMenu.css('margin-left', $dropdownHeader.outerWidth() - $dropdownMenu.outerWidth() + 'px');
        }
        if (!visible) {
            $dropdownMenu.hide();
        }
    };
    
    // Helper functions
    var isMobile = function() {
        return window.matchMedia('(max-width: 767px)').matches;
    };
    
    var isTablet = function () {
        return window.matchMedia('(min-width: 768px) and (max-width: 1111px)').matches;
    };

    var isDesktop = function() {
        return window.matchMedia('(min-width: 1112px)').matches;
    };
    
    var ensureDesktopDropdownWidth = function ($dropdownHeader, $dropdownMenu) {
        $dropdownMenu.css('min-width', $dropdownHeader.outerWidth() + 'px');
    };
    
    // Init all dropdowns
    $('.dropdown-header').each(function (elementPosition, headerDiv) {
        var $dropdownHeader = $(headerDiv);
        var $dropdownMenu = $dropdownHeader.next('nav').children('.dropdown-items-list');

        $(window).on('resize', function () {
            handleResponsivness($dropdownHeader, $dropdownMenu);
        });

        $dropdownHeader.on('click', function () {
            toogleDropdown($dropdownHeader, $dropdownMenu);
        });
    });
    $(window).on('mouseup', handleGlobalClick);
});