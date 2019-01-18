(function () {
    window.kentico.pageBuilder.registerInlineEditor("slideshow-uploader-editor", {
        init: function (options) {
            var editor = options.editor;
            var swiper = window.kenticoPageBuilder.getSwiper(editor.getAttribute("data-swiper-id"));
            var plusButton = swiper.querySelector(".sweeper-plus");
            var minusButton = swiper.parentElement.querySelector(".sweeper-minus");

            var addSlide = function (swiper, index) {
                var tempId = generateUuid();
                var markup = buildSlideMarkup(tempId);
                swiper.addSlide(index, markup);

                var dropZone = new Dropzone(editor.parentElement.querySelector("div#" + tempId + ".dropzone"), {
                    url: editor.getAttribute("data-upload-url"),
                    clickable: editor.parentElement.querySelector("div#" + tempId + ".dropzone a.dz-clickable")
                });

                dropZone.on("success",
                    function (e) {
                        var content = JSON.parse(e.xhr.response);
                        var newId = content.guid;
                        replaceId(dropZone, newId);
                        var slideData = collectSlideData(swiper);

                        var event = new CustomEvent("updateProperty",
                            {
                                detail: {
                                    name: options.propertyName,
                                    value: slideData
                                }
                            });

                        editor.dispatchEvent(event);
                    });
            };

            var buildSlideMarkup = function (tempId) {
                return '<div class="swiper-slide"><div class="dropzone" id="' + tempId + '">Slide ' + tempId + ' - <a class="dz-clickable">Upload a file</a></div></div>';
            };

            var generateUuid = function () {
                return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                    var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);

                    return v.toString(16);
                });
            };

            var replaceId = function (htmlElement, newId) {
                htmlElement.id = newId;

                return htmlElement; // TODO necessary?
            };

            var removeSlide = function (swiper, index) {
                swiper.removeSlide(index);

                // TODO collect guids and dispatch event
            };

            var collectSlideData = function (swiper) {
                var output = [];
                //var i = 0;

                swiper.slides.forEach(function (slide) {
                    //var slideData = {
                    //    index = i,
                    //    imageGuid = 
                    //};
                    var childDropzone = slide.querySelector(".dropzone");
                    output.push(childDropzone.id);
                });

                return output;
            };

            plusButton.onClick = addSlide(swiper, swiper.activeIndex);
            minusButton.onClick = removeSlide(swiper, swiper.activeIndex);
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
