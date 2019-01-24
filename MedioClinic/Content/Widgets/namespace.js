(function (kenticoPageBuilder, undefined) {
    var swipers = [];

    kenticoPageBuilder.addSwiper = function (id, swiper) {
        var found = false;
        var foundPosition = -1;

        for (var i = 0; i <= swipers.length - 1; i++) {
            if (swipers[i].id === id) {
                found = true;
                foundPosition = i;
            }
        }

        if (found) {
            return swipers[foundPosition].id;
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
        for (var i = 0; i <= swipers.length - 1; i++) {
            if (swipers[i].id === id) {
                return swipers[i].swiper;
            }
        }
    };

    kenticoPageBuilder.removeSwiper = function (id) {
        for (var i = swipers.length - 1; i >= 0; i--) {
            if (swipers[i].id === id) {
                swipers.splice(i, 1);
            }
        }
    };

    kenticoPageBuilder.initSwiper = function (swiperId, loop) {
        var swiper = new Swiper("#" + swiperId, {
            loop: loop,
            speed: 300,
            navigation: {
                nextEl: "#" + swiperId + " .swiper-button-next",
                prevEl: "#" + swiperId + " .swiper-button-prev"
            },
            effect: "fade",
            fadeEffect: {
                crossFade: true
            }
        });

        kenticoPageBuilder.addSwiper(swiperId, swiper);
    };
}(window.kenticoPageBuilder = window.kenticoPageBuilder || {}, undefined));