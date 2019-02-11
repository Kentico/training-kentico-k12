(function () {
    window.kentico.pageBuilder.registerInlineEditor("image-uploader-editor", {
        init: function (options) {
            var editor = options.editor;
            var zone = editor.querySelector(".dz-uploader");
            var clickable = editor.querySelector(".dz-clickable");

            var dropzone = new Dropzone(zone,
                {
                    acceptedFiles: medioClinic.dropzoneCommon.acceptedFiles,
                    maxFiles: 1,
                    url: editor.getAttribute("data-upload-url"),
                    createImageThumbnails: false,
                    clickable: clickable,
                    dictInvalidFileType: options.localizationService.getString(
                        "InlineEditors.ImageUploaderEditor.InvalidFileType")
                });

            dropzone.on("success",
                function (e) {
                    var content = JSON.parse(e.xhr.response);

                    var event = new CustomEvent("updateProperty",
                        {
                            detail: {
                                value: content.guid,
                                name: options.propertyName
                            }
                        });

                    editor.dispatchEvent(event);
                });

            dropzone.on("error",
                function (e) {
                    document.querySelector(".dz-preview").style.display = "none";
                    medioClinic.dropzoneCommon.processErrors(e.xhr.status);
                });
        },

        destroy: function (options) {
            var dropZone = options.editor.querySelector(".dz-uploader").dropzone;
            if (dropZone) {
                dropZone.destroy();
            }
        }
    });
})();
