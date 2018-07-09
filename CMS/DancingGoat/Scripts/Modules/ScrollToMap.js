define(['domReady!', 'jQuery'], function (document, $) {

    var google = window.google,
        mapObject,
        $addressElements = $('.js-scroll-to-map'),
        $mapWrapper = $('.js-map-wrapper'),
        mobileRedirect = $mapWrapper.hasClass('mobile-map-redirect'),
        mapObjectIdentifier = $mapWrapper.children('div:not([class^="Webpart"])').attr('id'),

        // There is no simple way to access map object directly,
        // because the object is a global object and its name
        // is the map div wrapper's IDentifier        
        loadMapObject = function () {
            return mapObject = window[mapObjectIdentifier];
        },

        scrollToLocation = function (location) {
            var geocoder = new google.maps.Geocoder();
            $("html, body").animate({ scrollTop: $(".map-title").offset().top }, 400);
            geocoder.geocode({ 'address': location }, scrollToMapPosition);
        },

        scrollToMapPosition = function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                var location = results[0].geometry.location;
                if (window.matchMedia('(min-width: 768px)').matches || !mobileRedirect) {
                    mapObject.setCenter(new google.maps.LatLng(location.lat(), location.lng()));
                    mapObject.setZoom(16);
                } else {
                    redirectToMobileApp(location);
                }
            }
        },

        redirectToMobileApp = function (location) {
            if (navigator.userAgent.match(/iPhone/i)) {
                window.location = 'http://maps.apple.com/?q=' + encodeURIComponent(location.lat() + ',' + location.lng());
            } else {
                window.location = 'http://maps.google.com/maps?q=' + encodeURIComponent(location.lat() + ',' + location.lng());
            }
        },

        scrollMap = function ($element) {
            var elAddress = $element.data().address;

            if (!mapObject) {
                loadMapObject();
            }

            $addressElements.removeClass('selected');
            $element.addClass('selected');
            scrollToLocation(elAddress);
        },

        scrollMapFromUrl = function () {
            var officeName = location.hash.slice(1);

            if ($addressElements.length && officeName) {
                var $elementWithData = $addressElements.filter('[data-address-id="' + officeName + '"]');

                if ($elementWithData.length) {
                    if (loadMapObject()) {
                        scrollMap($elementWithData);
                    } else {
                        $(window).load(function () {
                            scrollMap($elementWithData);
                        });
                    }
                }
            }
        };

    scrollMapFromUrl();
    $(window).on('hashchange', scrollMapFromUrl);

    $addressElements.each(function (elementPosition, element) {
        var $element = $(element);

        $element.on('click', function () {
            var newHash = $element.attr('data-address-id');
            if (newHash) {
                location.hash = newHash;
            } else {
                scrollMap($element);
            }
        });
    });
});