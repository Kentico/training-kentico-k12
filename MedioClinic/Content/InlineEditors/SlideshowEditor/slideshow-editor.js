(function () {
    window.kentico.pageBuilder.registerInlineEditor("slideshow-editor", {
        init: function (options) {
            var imageGuidPrefix = "i-";
            var editor = options.editor;
            var plusButton = editor.parentElement.querySelector("ul.kn-slideshow-buttons .kn-swiper-plus");
            var minusButton = editor.parentElement.querySelector("ul.kn-slideshow-buttons .kn-swiper-minus");

            // Image rendering: Alternative 2 (begin)
            var imageGuids = editor.getAttribute("data-image-guids").split(";");
            imageGuids.splice(-1, 1);
            // Image rendering: Alternative 2 (end)

            /** Adds a new slide to the Swiper object, together with a new Dropzone object. */
            var addSlide = function () {
                var swiper = medioClinic
                    .slideshowWidget
                    .getCurrentSwiper(editor, medioClinic.slideshowWidget.swiperGuidAttribute);
                var tempGuid = generateUuid();
                var tempId = imageGuidPrefix + tempGuid;
                var markup =
                    buildSlideMarkup(tempId, options.localizationService.getString("InlineEditors.Dropzone.DropText"));
                var activeIndexWhenAdded = swiper.slides.length > 0 ? swiper.activeIndex + 1 : 0;

                // Image rendering: Alternative 2
                imageGuids.splice(activeIndexWhenAdded, 0, tempGuid);

                swiper.addSlide(activeIndexWhenAdded, markup);
                swiper.slideNext();

                // Image rendering: Alternative 2
                var previewTemplate = "<div class=\"dz-preview dz-file-preview\"><img data-dz-thumbnail /></div>";

                var dropzone = new Dropzone(editor.parentElement.querySelector("div#" + tempId + ".dropzone"), {
                    acceptedFiles: medioClinic.dropzoneCommon.acceptedFiles,
                    maxFiles: 1,
                    url: editor.getAttribute("data-upload-url"),
                    clickable: editor.parentElement.querySelector("div#" + tempId + ".dropzone a.dz-clickable"),
                    dictInvalidFileType: options.localizationService.getString(
                        "MedioClinic.InlineEditors.SlideshowEditor.InvalidFileType"),

                    // Image rendering: Alternative 2 (begin)
                    previewsContainer: swiper.slides[activeIndexWhenAdded],
                    previewTemplate: previewTemplate,
                    thumbnailWidth: editor.getAttribute("data-width"),
                    thumbnailHeight: editor.getAttribute("data-height")
                    // Image rendering: Alternative 2 (end)
                });

                dropzone.on("success",
                    function (e) {
                        var content = JSON.parse(e.xhr.response);
                        var newGuid = content.guid;
                        replaceId(dropzone.element, imageGuidPrefix + newGuid);
                        hideDropzoneLabels(dropzone.element);

                        // Image rendering: Alternative 1 (begin)
                        /*var slideIdsAfterUpload = medioClinic.slideshowWidget.collectDropzoneIds(swiper);

                        var imageGuids = slideIdsAfterUpload.map(function (slideId) {
                            return getGuidFromId(slideId);
                        });*/
                        // Image rendering: Alternative 1 (end)

                        var dropzoneIndex = getDropzoneElementIndex(dropzone.element);
                        imageGuids.splice(dropzoneIndex, 1, newGuid);
                        dispatchBuilderEvent(imageGuids);
                    });


                dropzone.on("error",
                    function (e) {
                        medioClinic.dropzoneCommon.processErrors(e.xhr.status);
                    });
            };

            /**
             * Hides the clickable element and the instructional message in the Dropzone object.
             * @param {HTMLElement} dropzoneElement The parent HTML element of the Dropzone object.
             */
            var hideDropzoneLabels = function (dropzoneElement) {
                dropzoneElement.querySelector("a.dz-clickable").style.display = "none";
                dropzoneElement.querySelector(".dz-message").style.display = "none";
            };

            // Image rendering: Alternative 1 (begin)
            /**
             * Removes any prefixes that had been previously concatedated in front of a GUID.
             * @param {string} id The GUID with the prefix.
             * @returns {string} The bare GUID value.
             */
            /*var getGuidFromId = function (id) {
                return id.slice(-36);
            };*/
            // Image rendering: Alternative 1 (end)

            /**
             * Gets the position (index) of a given Dropzone HTML element in the parent Swiper element
             * @param {HTMLElement} dropzoneElement The HTML element of the Dropzone object.
             * @returns {number} The position in the parent Swiper.
             */
            var getDropzoneElementIndex = function (dropzoneElement) {
                return Array.prototype.slice.call(dropzoneElement.parentElement.parentElement.children)
                    .indexOf(dropzoneElement.parentElement);
            };

            /**
             * Crafts an HTML markup of a new Swiper slide, together with its child Dropzone element.
             * @param {string} id The ID of the future Dropzone HTML element.
             * @param {string} dropText The instructional text for the Dropzone object.
             * @returns {string} The complete HTML markup of the Swiper slide.
             */
            var buildSlideMarkup = function (id, dropText) {
                return "<div class=\"swiper-slide dropzone-previews\"><div class=\"dropzone\" id=\""
                    + id + "\"><div class=\"dz-message\">" + dropText + "</div></div></div>";
            };

            /** 
             *  Generates an UUID (GUID).
             *  @returns {string} The UUID.
             * */
            var generateUuid = function () {
                return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function (c) {
                    var r = Math.random() * 16 | 0, v = c === "x" ? r : r & 0x3 | 0x8;

                    return v.toString(16);
                });
            };

            /**
             * Replaces an ID of a given HTML element.
             * @param {HTMLElement} htmlElement The HTML element, which ID should be swapped.
             * @param {string} newId The new ID.
             */
            var replaceId = function (htmlElement, newId) {
                htmlElement.id = newId;
            };

            /**
             * Dispatches the Kentico page builder event that updates state of the widget in the browser store.
             * @param {string[]} imageGuids The GUIDs of the images in the Swiper object.
             */
            var dispatchBuilderEvent = function (imageGuids) {
                var event = new CustomEvent("updateProperty",
                    {
                        detail: {
                            name: options.propertyName,
                            value: imageGuids,

                            // Image rendering: Alternative 2
                            refreshMarkup: false
                        }
                    });

                editor.dispatchEvent(event);
            };

            /** Removes a slide from the current Swiper object. */
            var removeSlide = function () {
                var swiper = medioClinic
                    .slideshowWidget
                    .getCurrentSwiper(editor, medioClinic.slideshowWidget.swiperGuidAttribute);

                // Image rendering: Alternative 1 (begin)
                /*var dropzoneIds = medioClinic.slideshowWidget.collectDropzoneIds(swiper);

                var imageGuids = dropzoneIds.map(function (slideId) {
                    return getGuidFromId(slideId);
                });*/
                // Image rendering: Alternative 1 (end)

                var dropzoneElement = swiper.slides[swiper.activeIndex].children[0];
                var dropzoneIndex = getDropzoneElementIndex(dropzoneElement);

                if (imageGuids[dropzoneIndex]) {
                    var body = "attachmentGuid=" + imageGuids[dropzoneIndex];
                    var xhr = new XMLHttpRequest();
                    xhr.open("DELETE", editor.getAttribute("data-delete-url"), true);
                    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

                    xhr.onreadystatechange = function () {
                        if (xhr.readyState === 4 && xhr.status === 204) {
                            console.warn("Could not remove the slide image from page attachments.");
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
            var swiper = medioClinic
                .slideshowWidget
                .getCurrentSwiper(options.editor, medioClinic.slideshowWidget.swiperGuidAttribute);

            if (swiper) {
                var dropzoneIds = medioClinic.slideshowWidget.collectDropzoneIds(swiper);

                if (dropzoneIds && Array.isArray(dropzoneIds)) {
                    dropzoneIds.forEach(function (dropzoneId) {
                        var dropzoneElement = document.getElementById(dropzoneId);

                        if (dropzoneElement && dropzoneElement.dropzone) {
                            dropzoneElement.dropzone.destroy();
                        }
                    });
                }

                medioClinic.slideshowWidget.removeSwiper(swiper.el.id);
                swiper.destroy();
            }
        }
    });
})();
