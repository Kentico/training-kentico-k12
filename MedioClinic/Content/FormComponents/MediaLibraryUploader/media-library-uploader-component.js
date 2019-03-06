window.medioClinic = window.medioClinic || {};

(function (mediaLibraryUploaderComponent) {
    /**
     * Displays file size and MIME type of the selected file.
     * @param {HTMLElement} target HTML element that invoked the function.
     */
    mediaLibraryUploaderComponent.renderFileDetails = function (target) {
        var mbSize = 1048576;
        var file = target.files[0];

        if (file) {
            var detailsElement =
                target.parentElement.parentElement.querySelector(".kn-upload-file-details");
            var uploadButton =
                target.parentElement.parentElement.parentElement.querySelector(".upload-button button");

            if (file.type === "image/jpeg" || file.type === "image/png") {
                var fileSize = 0;
                uploadButton.disabled = false;

                if (file.size > mbSize) {
                    fileSize = (Math.round(file.size * 100 / mbSize) / 100).toString() + 'MB';
                } else {
                    fileSize = (Math.round(file.size * 100 / 1024) / 100).toString() + 'kB';
                }

                detailsElement.querySelector(".kn-file-size").innerHTML =
                    "<strong>Size:</strong> "
                    + fileSize;
                detailsElement.querySelector(".kn-file-type").innerHTML =
                    "<strong>Type:</strong> "
                    + file.type;
            } else {
                detailsElement.querySelector(".kn-file-type").innerHTML =
                    "<strong>Type:</strong> Invalid file type. Please upload a .jpg or .png file.";

                uploadButton.disabled = true;
            }
        }
    };

    /**
     * Uploads the selected file using XHR.
     * @param {HTMLElement} target HTML element that invoked the function.
     * @param {string} url URL to upload the file to.
     */
    mediaLibraryUploaderComponent.uploadFile = function (target, url) {
        if (url && url.length > 0) {
            var xhr = new XMLHttpRequest();
            var parentForm = getParentForm(target, null);
            var formData = new FormData(parentForm);
            xhr.addEventListener("load", onUploadCompleted, false);
            xhr.addEventListener("progress", onUploadProgressChange, false);
            xhr.addEventListener("error", onUploadFailed, false);
            xhr.open("POST", url);
            xhr.send(formData);
        }
    };

    /**
     * Searches for nearest parent form HTML element.
     * @param {HTMLElement} target HTML element that invoked the function.
     * @param {HTMLElement=} mostParentElement An optional HTML element where searching should stop.
     * @returns {HTMLElement} The parent form element, or null.
     */
    var getParentForm = function (target, mostParentElement) {
        if (!mostParentElement) {
            mostParentElement = document.getElementsByTagName("body")[0];
        }

        if (target !== mostParentElement) {
            var parent = target.parentElement;

            if (parent.tagName === "FORM") {
                return parent;
            } else {
                return getParentForm(parent, mostParentElement);
            }
        } else {
            return null;
        }
    };

    /**
     * Handles error codes
     * @param {number} statusCode HTTP status code
     */
    var processErrors = function (statusCode) {
        var errorFlag = "error";

        if (statusCode >= 500) {
            window.medioClinic.showMessage(
                "The upload of the image failed. Please contact the system administrator.",
                errorFlag);
        } else if (statusCode === 422) {
            window.medioClinic.showMessage(
                "The uploaded image could not be processed. Please contact the system administrator.",
                errorFlag);
        } else {
            window.medioClinic.showMessage(
                "An unknown error happened. Please contact the system administrator.",
                errorFlag);
        }
    };

    /**
     * Gets the media library file GUID and puts its value into another form input element.
     * @param {Event} e Event invoked when an upload is complete.
     */
    var onUploadCompleted = function (e) {
        if (e.target.status >= 200 && e.target.status < 300) {
            var responseObject = JSON.parse(e.target.response);
            var filePathElement = document.getElementById(responseObject.filePathId);
            filePathElement.value = responseObject.fileGuid;
            window.medioClinic.showMessage(
                "Upload of the image is complete. File GUID: "
                + responseObject.fileGuid, "info");
        } else {
            processErrors(e.target.status);
        }
    };

    /**
     * Logs the file upload progress into the console.
     * @param {Event} e Event invoked when the upload progress changes.
     */
    var onUploadProgressChange = function (e) {
        var percentComplete = Math.round(e.loaded * 100 / e.total);
        console.info(
            "Upload progress: "
            + percentComplete + "%");
    };

    /**
     * Logs the failed upload to the console.
     * @param {Event} e Event invoked when the upload fails.
     */
    var onUploadFailed = function (e) {
        processErrors(e.target.status);
    };
}(window.medioClinic.mediaLibraryUploaderComponent = window.medioClinic.mediaLibraryUploaderComponent || {}));