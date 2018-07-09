define(['domReady!', 'jQuery'], function (domReady, $) {

    var $productFilter,
        $productPage,
        $toggleBtn = $("#filter-drop-down");

    $(window).on('resize', handleMobileMenu);
    $toggleBtn.on('click', toogleDropdown);

    bindSelectorsInUpdatePanel();
    init();

    // Init values
    function init() {
        $productFilter.data("closed", "true");
    }

    //On UpdatePanel Refresh
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    if (prm != null) {
        prm.add_endRequest(function (sender, e) {
            if (sender._postBackSettings.panelsToUpdate != null) {
                // Rebind selectors and show filter
                bindSelectorsInUpdatePanel();
                showProductFilter();
            }
        });
    };

    function bindSelectorsInUpdatePanel() {
        $productFilter = $(".product-filter");
        $productPage = $(".product-page");
    }

    function handleMobileMenu() {
        if (isMobile() && $productFilter.data("closed") === "true") {
            hideProductFilter();
        } else {
            showProductFilter();
        }
    }

    function toogleDropdown() {
        if (isProductFilterVisible()) {
            hideProductFilter(true);
            return;
        }
        showProductFilter(true);
    }

    function showProductFilter(removeCloseFlag) {
        $productFilter.removeClass("hidden");

        if (removeCloseFlag) {
            $productFilter.data("closed", "");
        }

        if (!isMobile()) {
            $productPage.removeClass("no-margin");
        } else {
            $productPage.addClass("no-margin");
        }
    }

    function hideProductFilter(closedFlag) {
        if (closedFlag) {
            $productFilter.data("closed", "true");
        }
        $productFilter.addClass("hidden");
        $productPage.removeClass("no-margin");
    }

    function isProductFilterVisible() {
        return !$productFilter.hasClass("hidden");
    }

    function isMobile() {
        return window.matchMedia('(max-width: 767px)').matches;
    }
});