window.medioClinic = window.medioClinic || {};

(function (dropzoneCommon) {
    /** List of files accepted by the Dropzone object. */
    dropzoneCommon.acceptedFiles = ".bmp, .gif, .ico, .png, .wmf, .jpg, .jpeg, .tiff, .tif";

    /**
     * Handles error codes
     * @param {number} statusCode HTTP status code
     */
    dropzoneCommon.processErrors = function (statusCode) {
        var errorFlag = "error";

        if (statusCode >= 500) {
            medioClinic.showMessage(kentico.localization.strings["FormComponent.DropzoneCommon.UploadFailed"], errorFlag);
        } else if (statusCode === 422) {
            medioClinic.showMessage(kentico.localization.strings["FormComponent.DropzoneCommon.UploadUnprocessable"], errorFlag);
        } else {
            medioClinic.showMessage(kentico.localization.strings["FormComponent.DropzoneCommon.UploadUnknownError"], errorFlag);
        }
    };
}(window.medioClinic.dropzoneCommon = window.medioClinic.dropzoneCommon || {}));