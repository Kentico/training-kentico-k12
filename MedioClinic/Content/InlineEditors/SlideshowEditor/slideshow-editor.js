(function () {
    window.kentico.pageBuilder.registerInlineEditor("slideshow-editor", {
        init: function (options) {
            var imageGuidPrefix = "i-";
            var editor = options.editor;
            var plusButton = editor.parentElement.querySelector(".swiper-plus");
            var minusButton = editor.parentElement.querySelector(".swiper-minus");

            // Image rendering: Alternative 2 (begin)
            /*var imageGuids = editor.getAttribute("data-image-guids").split(";");
            imageGuids.splice(-1, 1);*/
            // Image rendering: Alternative 2 (end)

            var addSlide = function () {
                var swiper = window.kenticoPageBuilder.getCurrentSwiper(editor, kenticoPageBuilder.swiperGuidAttribute);
                var tempGuid = generateUuid();
                var tempId = imageGuidPrefix + tempGuid;
                var markup = buildSlideMarkup(tempId);
                var activeIndexWhenAdded = swiper.slides.length > 0 ? swiper.activeIndex + 1 : 0;

                // Image rendering: Alternative 2
                /*imageGuids.splice(activeIndexWhenAdded, 0, tempGuid);*/

                swiper.addSlide(activeIndexWhenAdded, markup);
                swiper.slideNext();

                // Image rendering: Alternative 2
                /*var previewTemplate = "<div class=\"dz-preview dz-file-preview\"><img data-dz-thumbnail /></div>";*/

                var dropzone = new Dropzone(editor.parentElement.querySelector("div#" + tempId + ".dropzone"), {
                    url: editor.getAttribute("data-upload-url"),
                    clickable: editor.parentElement.querySelector("div#" + tempId + ".dropzone a.dz-clickable")//,

                    // Image rendering: Alternative 2 (begin)
                    /*previewsContainer: swiper.slides[activeIndexWhenAdded],
                    previewTemplate: previewTemplate,
                    thumbnailWidth: editor.getAttribute("data-width"),
                    thumbnailHeight: editor.getAttribute("data-height")*/
                    // Image rendering: Alternative 2 (end)
                });

                dropzone.on("success",
                    function (e) {
                        var content = JSON.parse(e.xhr.response);
                        var newGuid = content.guid;
                        replaceId(dropzone.element, imageGuidPrefix + newGuid);

                        // Image rendering: Alternative 1 (begin)
                        var slideIdsAfterUpload = window.kenticoPageBuilder.collectDropzoneIds(swiper);

                        var imageGuids = slideIdsAfterUpload.map(function (slideId) {
                            return getGuidFromId(slideId);
                        });
                        // Image rendering: Alternative 1 (end)

                        var dropzoneIndex = getDropzoneElementIndex(dropzone.element);
                        imageGuids.splice(dropzoneIndex, 1, newGuid);
                        dispatchBuilderEvent(imageGuids);
                    });
            };

            var getGuidFromId = function (id) {
                return id.slice(-36);
            };

            var getDropzoneElementIndex = function (dropzone) {
                return Array.prototype.slice.call(dropzone.parentElement.parentElement.children)
                    .indexOf(dropzone.parentElement);
            };

            var buildSlideMarkup = function (tempId) {
                return '<div class="swiper-slide dropzone-previews"><div class="dropzone" id="' + tempId + '">Slide '
                    + tempId + ' - <a class="dz-clickable">Upload a file</a></div></div>'; // TODO escape quotes
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

            var dispatchBuilderEvent = function (imageGuids) {
                var event = new CustomEvent("updateProperty",
                    {
                        detail: {
                            name: options.propertyName,
                            value: imageGuids//,

                            // Image rendering: Alternative 2
                            /*refreshMarkup: false*/
                        }
                    });

                editor.dispatchEvent(event);
            };

            var removeSlide = function () {
                var swiper = window.kenticoPageBuilder.getCurrentSwiper(editor, kenticoPageBuilder.swiperGuidAttribute);

                // Image rendering: Alternative 1 (begin)
                var dropzoneIds = window.kenticoPageBuilder.collectDropzoneIds(swiper);

                var imageGuids = dropzoneIds.map(function (slideId) {
                    return getGuidFromId(slideId);
                });
                // Image rendering: Alternative 1 (end)

                var dropzoneElement = swiper.slides[swiper.activeIndex].children[0];
                var dropzoneIndex = getDropzoneElementIndex(dropzoneElement);

                if (imageGuids[dropzoneIndex]) {
                    var body = "attachmentGuid=" + imageGuids[dropzoneIndex];
                    var xhr = new XMLHttpRequest();
                    xhr.open("POST", editor.getAttribute("data-delete-url"), true);
                    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

                    xhr.onreadystatechange = function () {
                        if (xhr.readyState === 4 && xhr.status === 204) {
                            throw new Error("Could not delete image");
                        }
                    };

                    xhr.send(body);
                }

                imageGuids.splice(dropzoneIndex, 1);
                swiper.removeSlide(swiper.activeIndex);
                dispatchBuilderEvent(imageGuids);
            };

            plusButton.addEventListener("click", addSlide);
            minusButton.addEventListener("click", removeSlide);
        },

        destroy: function (options) {
            var swiper = window.kenticoPageBuilder.getCurrentSwiper(options.editor, kenticoPageBuilder.swiperGuidAttribute);

            if (swiper) {
                var dropzoneIds = window.kenticoPageBuilder.collectDropzoneIds(swiper);

                if (dropzoneIds && Array.isArray(dropzoneIds)) {
                    dropzoneIds.forEach(function (dropzoneId) {
                        var dropzoneElement = document.getElementById(dropzoneId);

                        if (dropzoneElement && dropzone.dropzone) {
                            dropzoneElement.dropzone.destroy();
                        }
                    });
                }

                window.kenticoPageBuilder.removeSwiper(swiper.el.id);
                swiper.destroy();
            }
        }
    });
})();
