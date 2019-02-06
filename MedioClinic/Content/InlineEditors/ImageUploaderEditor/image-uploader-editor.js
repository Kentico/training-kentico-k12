(function () {
  window.kentico.pageBuilder.registerInlineEditor("image-uploader-editor", {
    init: function (options) {
      var editor = options.editor;
      var zone = editor.querySelector(".uploader");
      var clickable = editor.querySelector(".dz-clickable");

      var dropZone = new Dropzone(zone,
        {
          acceptedFiles: ".bmp, .gif, .ico, .png, .wmf, .jpg, .jpeg, .tiff, .tif",
          maxFiles: 1,
          url: editor.getAttribute("data-upload-url"),
          createImageThumbnails: false,
          clickable: clickable,
          dictInvalidFileType: options.localizationService.getString(
            "MedioClinic.InlineEditors.ImageUploaderEditor.InvalidFileType")
        });

      dropZone.on("success",
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
    },

    destroy: function (options) {
      var dropZone = options.editor.querySelector(".uploader").dropzone;
      if (dropZone) {
        dropZone.destroy();
      }
    }
  });
})();
