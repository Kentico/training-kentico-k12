(function () {
    window.kentico.pageBuilder.registerInlineEditor("image-uploader-editor", {
        init: function (options) {
            var editor = options.editor;
            var zone = editor.querySelector(".dz-uploader");
            var clickable = editor.querySelector(".dz-clickable");

            var dropzone = new Dropzone(zone, {
                    acceptedFiles: window.medioClinic.dropzoneCommon.acceptedFiles,
                    maxFiles: 1,
                    url: editor.getAttribute("data-upload-url"),
                    createImageThumbnails: false,
                    clickable: clickable,
                    dictInvalidFileType: options.localizationService.getString(
                        "InlineEditors.Dropzone.InvalidFileType")
                });

            dropzone.on("success",
                function (event) {
                    var content = JSON.parse(event.xhr.response);

                    var customEvent = new CustomEvent("updateProperty",
                        {
                            detail: {
                                value: content.guid,
                                name: options.propertyName
                            }
                        });

                    editor.dispatchEvent(customEvent);
                });

            dropzone.on("error",
                function (event) {
                    document.querySelector(".dz-preview").style.display = "none";
                    window.medioClinic.dropzoneCommon.processErrors(event.xhr.status, options.localizationService);
                });
        },

        destroy: function (options) {
            var dropzone = options.editor.querySelector(".dz-uploader").dropzone;

            if (dropzone) {
                dropzone.destroy();
            }
        }
    });
})();
