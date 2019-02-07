window.medioClinic = window.medioClinic || {};

(function (slideshowWidget, undefined) {
    var swipers = [];
    slideshowWidget.swiperGuidAttribute = "data-swiper-guid";

    slideshowWidget.addSwiper = function (id, swiper) {
        var found = medioClinic.slideshowWidget.getSwiper(id);

        if (found) {
            return found[0].id;
        } else {
            var swiperToAdd = {
                id: id,
                swiper: swiper
            };

            swipers.push(swiperToAdd);

            return id;
        }
    };

    slideshowWidget.getSwiper = function (id) {
        var found = swipers.filter(function (currentSwiper) {
            return currentSwiper.id === id;
        });

        if (found.length > 0) {
            return found[0];
        } else {
            return null;
        }
    };

    slideshowWidget.removeSwiper = function (id) {
        for (var i = swipers.length - 1; i >= 0; i--) {
            if (swipers[i].id === id) {
                swipers.splice(i, 1);
            }
        }
    };

    slideshowWidget.initSwiper = function (swiperId, editMode, transitionDelay, transitionSpeed) {
        var swiperSelector = "#" + swiperId;

        var configuration = {
            loop: !editMode,
            speed: transitionSpeed,
            navigation: {
                nextEl: "#" + swiperId + " .swiper-button-next",
                prevEl: "#" + swiperId + " .swiper-button-prev"
            },
            effect: "fade",
            fadeEffect: {
                crossFade: true
            },
            autoHeight: true
        };

        if (!editMode) {
            configuration["autoplay"] = {
                delay: transitionDelay,
                disableOnInteraction: true
            };
        }

        var swiper = new Swiper(swiperSelector, configuration);
        medioClinic.slideshowWidget.addSwiper(swiperId, swiper);
    };

    slideshowWidget.getCurrentSwiper = function (editor, swiperGuidAttribute) {
        // Retrieving via the "swiper" property of the respective HTML element
        //return editor.parentElement.swiper;

        // Retrieving off of the global namespace container
        return medioClinic.slideshowWidget.getSwiper(editor.getAttribute(swiperGuidAttribute)).swiper;
    };

    slideshowWidget.collectDropzoneIds = function (swiper) {
        var output = [];

        for (var s = 0; s <= swiper.slides.length - 1; s++) {
            var childDropzone = swiper.slides[s].children[0];
            output.push(childDropzone.id);
        }

        return output;
    };
}(window.medioClinic.slideshowWidget = window.medioClinic.slideshowWidget || {}, undefined));
