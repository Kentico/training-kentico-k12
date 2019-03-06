window.medioClinic = window.medioClinic || {};

(function (dropzoneCommon) {
    /** List of files accepted by the Dropzone object. */
    dropzoneCommon.acceptedFiles = ".bmp, .gif, .ico, .png, .wmf, .jpg, .jpeg, .tiff, .tif";

    /**
     * Handles error codes
     * @param {number} statusCode HTTP status code
     * @param {object} localizationService Kentico localization service
     */
    dropzoneCommon.processErrors = function (statusCode, localizationService) {
        var errorFlag = "error";

        if (statusCode >= 500) {
            window.medioClinic.showMessage(localizationService.getString("FormComponent.DropzoneCommon.UploadFailed"), errorFlag);
        } else if (statusCode === 422) {
            window.medioClinic.showMessage(localizationService.getString("FormComponent.DropzoneCommon.UploadUnprocessable"), errorFlag);
        } else {
            window.medioClinic.showMessage(localizationService.getString("FormComponent.DropzoneCommon.UploadUnknownError"), errorFlag);
        }
    };
}(window.medioClinic.dropzoneCommon = window.medioClinic.dropzoneCommon || {}));