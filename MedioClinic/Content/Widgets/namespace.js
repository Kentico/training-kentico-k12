(function (kenticoPageBuilder, undefined) {
    var swipers = [];
    kenticoPageBuilder.swiperGuidAttribute = "data-swiper-guid";

    kenticoPageBuilder.addSwiper = function (id, swiper) {
        //var found = false;
        //var foundPosition = -1;

        //for (var i = 0; i <= swipers.length - 1; i++) {
        //    if (swipers[i].id === id) {
        //        found = true;
        //        foundPosition = i;
        //    }
        //}

        var found = kenticoPageBuilder.getSwiper(id);

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

    kenticoPageBuilder.getSwiper = function (id) {
        var found = swipers.filter(function (currentSwiper) {
            return currentSwiper.id === id;
        });

        if (found.length > 0) {
            return found[0];
        } else {
            return null;
        }
        //for (var i = 0; i <= swipers.length - 1; i++) {
        //    if (swipers[i].id === id) {
        //        return swipers[i].swiper;
        //    }
        //}
    };

    kenticoPageBuilder.removeSwiper = function (id) {
        for (var i = swipers.length - 1; i >= 0; i--) {
            if (swipers[i].id === id) {
                swipers.splice(i, 1);
            }
        }
    };

    kenticoPageBuilder.initSwiper = function (swiperId, editMode, transitionDelay, transitionSpeed) {
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
            }
        };

        if (!editMode) {
            configuration["autoplay"] = {
                delay: transitionDelay,
                disableOnInteraction: true
            };
        }

        var swiper = new Swiper(swiperSelector, configuration);
        kenticoPageBuilder.addSwiper(swiperId, swiper);
    };

    kenticoPageBuilder.getCurrentSwiper = function (editor, swiperGuidAttribute) {
        // Retrieving via the "swiper" property of the respective HTML element
        //return editor.parentElement.swiper;

        // Retrieving off of the global namespace container
        return kenticoPageBuilder.getSwiper(editor.getAttribute(swiperGuidAttribute)).swiper;
    };

    kenticoPageBuilder.collectDropzoneIds = function (swiper) { // TODO rename collectDropzoneIds ?
        var output = [];
        //var i = 0;

        for (var s = 0; s <= swiper.slides.length - 1; s++) {
            var childDropzone = swiper.slides[s].children[0];
            output.push(childDropzone.id);
        }

        //swiper.slides.forEach(function (slide) {
        //    //var slideData = {
        //    //    index = i,
        //    //    imageGuid = 
        //    //};
        //});

        return output;
    };
}(window.kenticoPageBuilder = window.kenticoPageBuilder || {}, undefined));