(function (medioClinic, undefined) {
    var swipers = [];
    medioClinic.swiperGuidAttribute = "data-swiper-guid"; // TODO Must be public?
    var onSwiperResize = function () {
        console.info(this);
    };

    medioClinic.addSwiper = function (id, swiper) {
        var found = medioClinic.getSwiper(id);

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

    medioClinic.getSwiper = function (id) {
        var found = swipers.filter(function (currentSwiper) {
            return currentSwiper.id === id;
        });

        if (found.length > 0) {
            return found[0];
        } else {
            return null;
        }
    };

    medioClinic.removeSwiper = function (id) {
        for (var i = swipers.length - 1; i >= 0; i--) {
            if (swipers[i].id === id) {
                swipers.splice(i, 1);
            }
        }
    };

    medioClinic.initSwiper = function (swiperId, editMode, transitionDelay, transitionSpeed) {
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
        medioClinic.addSwiper(swiperId, swiper);
    };

    medioClinic.getCurrentSwiper = function (editor, swiperGuidAttribute) {
        // Retrieving via the "swiper" property of the respective HTML element
        //return editor.parentElement.swiper;

        // Retrieving off of the global namespace container
        return medioClinic.getSwiper(editor.getAttribute(swiperGuidAttribute)).swiper;
    };

    medioClinic.collectDropzoneIds = function (swiper) {
        var output = [];

        for (var s = 0; s <= swiper.slides.length - 1; s++) {
            var childDropzone = swiper.slides[s].children[0];
            output.push(childDropzone.id);
        }

        return output;
    };
}(window.medioClinic = window.medioClinic || {}, undefined));