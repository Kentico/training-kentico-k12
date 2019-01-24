(function () {
    window.kentico.pageBuilder.registerInlineEditor("slideshow-uploader-editor", {
        init: function (options) {
            var editor = options.editor;
            var plusButton = editor.parentElement.querySelector(".swiper-plus");
            var minusButton = editor.parentElement.querySelector(".swiper-minus");

            var addSlide = function () {
                var swiper = getCurrentSwiper();
                var tempId = "i-" + generateUuid();
                var markup = buildSlideMarkup(tempId);
                swiper.addSlide(swiper.activeIndex + 1, markup);

                var dropZone = new Dropzone(editor.parentElement.querySelector("div#" + tempId + ".dropzone"), {
                    url: editor.getAttribute("data-upload-url"),
                    clickable: editor.parentElement.querySelector("div#" + tempId + ".dropzone a.dz-clickable")
                });

                dropZone.on("success",
                    function (e) {
                        var content = JSON.parse(e.xhr.response);
                        var newId = content.guid;
                        replaceId(dropZone.element, "i-" + newId);
                        var slideIds = collectSlideIds(swiper);

                        var slideGuids = slideIds.map(function (slideId) {
                            return slideId.slice(-36);
                        });

                        var event = new CustomEvent("updateProperty",
                            {
                                detail: {
                                    name: options.propertyName,
                                    value: slideGuids
                                }
                            });

                        editor.dispatchEvent(event);
                    });
            }; //.bind(this, 1);

            var getCurrentSwiper = function () {
                //return editor.parentElement.swiper; // Retrieving via the "swiper" property of the respective HTML element
                return window.kenticoPageBuilder.getSwiper(editor.getAttribute("data-swiper-id")); // Retrieving off of the global namespace container
            };

            var buildSlideMarkup = function (tempId) {
                return '<div class="swiper-slide"><div class="dropzone" id="' + tempId + '">Slide ' + tempId + ' - <a class="dz-clickable">Upload a file</a></div></div>'; // TODO escape quotes
            };

            var generateUuid = function () {
                return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function (c) {
                    var r = Math.random() * 16 | 0, v = c === "x" ? r : (r & 0x3 | 0x8);

                    return v.toString(16);
                });
            };

            var replaceId = function (htmlElement, newId) {
                htmlElement.id = newId;
            };

            var collectSlideIds = function (swiper) { // TODO rename collectSlideIds ?
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

            var removeSlide = function () {
                var swiper = getCurrentSwiper();
                swiper.removeSlide(swiper.activeIndex);

                // TODO collect guids and dispatch event
            };

            plusButton.addEventListener("click", addSlide);
            minusButton.addEventListener("click", removeSlide);
        },

        destroy: function (options) {
            // get current swiper
            // collect all dropzones
            // destroy them
            // window.kenticoPageBuilder.removeSwiper(...)

            //var dropZone = options.editor.querySelector(".uploader").dropzone;
            //if (dropZone) {
            //    dropZone.destroy();
            //}
        }
    });
})();
