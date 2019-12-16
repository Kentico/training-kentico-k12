window.medioClinic = window.medioClinic || {};

(function (mediaLibraryUploaderComponent) {
    /**
     * Displays file size and MIME type of the selected file.
     * @param {HTMLElement} target HTML element that invoked the function.
     */
    mediaLibraryUploaderComponent.onFileSelected = function (target) {
        var mbSize = 1048576;
        var file = target.files[0];
        var fileSizeString = "";
        var fileMimeTypeString = "";
        var detailsElement = target.parentElement.parentElement.querySelector(".kn-upload-file-details");
        var fileNameHiddenElement = detailsElement.querySelector(".kn-file-name-hidden");

        var uploadButton =
            target.parentElement.parentElement.parentElement.querySelector(".upload-button button");

        if (file) {
            if (file.type === "image/jpeg" || file.type === "image/png") {
                var fileSize = 0;
                uploadButton.disabled = false;

                if (file.size > mbSize) {
                    fileSize = (Math.round(file.size * 100 / mbSize) / 100).toString() + 'MB';
                } else {
                    fileSize = (Math.round(file.size * 100 / 1024) / 100).toString() + 'kB';
                }

                fileSizeString = fileSize.toString();
                fileMimeTypeString = file.type.toString();
            } else {
                fileMimeTypeString = null;
            }

            fileNameHiddenElement.value = file.name;
        }

        detailsElement.querySelector(".kn-file-size").innerHTML =
            "<strong>Size:</strong> "
            + fileSizeString;

        var fileTypeElement = detailsElement.querySelector(".kn-file-type");

        if (fileMimeTypeString) {
            fileTypeElement.innerHTML =
                "<strong>Type:</strong> "
                + fileMimeTypeString;
        } else {
            fileTypeElement.innerHTML
                = "<strong>Type:</strong> Invalid file type. Please upload a .jpg or .png file.";
            uploadButton.disabled = true;
        }
    };

    /**
     * Makes sure the user is warned about the need to re-upload a file in the form.
     * @param {string} fileGuidHiddenElementId ID of the hidden element with file GUID.
     * @param {string} fileNameHiddenElementId ID of the hidden element with file name.
     */
    mediaLibraryUploaderComponent.checkForUnuploadedFile = function (fileGuidHiddenElementId, fileNameHiddenElementId) {
        var fileGuidHiddenElement = document.getElementById(fileGuidHiddenElementId);
        var fileNameHiddenElement = document.getElementById(fileNameHiddenElementId);

        if (fileNameHiddenElement.value && !fileGuidHiddenElement.value) {
            var message = "The form was reloaded. Please select the file \"" + fileNameHiddenElement.value + "\" again and upload it.";
            processMessage(message, "warning", fileGuidHiddenElement.parentElement);
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
     * @param {number} statusCode HTTP status code.
     * @param {HTMLElement} targetElement The HTML element containing the form message element.
     */
    var processErrors = function (statusCode, targetElement) {
        var errorFlag = "error";

        if (statusCode >= 500) {
            processMessage(
                "The upload of the image failed. Please contact the system administrator.",
                errorFlag,
                targetElement);
        } else if (statusCode === 422) {
            processMessage(
                "The uploaded image could not be processed. Please contact the system administrator.",
                errorFlag,
                targetElement);
        } else {
            processMessage(
                "An unknown error happened. Please contact the system administrator.",
                errorFlag,
                targetElement);
        }
    };

    /**
     * Logs a console message and displays it in the page, if possible.
     * @param {string} message The message.
     * @param {string} type The type of the message.
     * @param {HTMLElement} targetElement The HTML element containing the form message element.
     */
    var processMessage = function (message, type, targetElement) {
        var cssClasses = "";

        if (typeof window.medioClinic.showMessage === "function") {
            window.medioClinic.showMessage(message, type, false);
        }

        if (type === "info") {
            cssClasses = "light-blue lighten-5";
            console.info(message);
        } else if (type === "warning") {
            cssClasses = "yellow lighten-3";
            console.warn(message);
        } else if (type === "error") {
            cssClasses = "red lighten-3";
            console.error(message);
        }

        if (targetElement) {
            var messageElement = targetElement.querySelector(".kn-form-messages");
            messageElement.appendChild(window.medioClinic.buildMessageMarkup(message, cssClasses));
        }
    };

    /**
     * Gets the media library file GUID and puts its value into another form input element.
     * @param {Event} e Event invoked when an upload is complete.
     */
    var onUploadCompleted = function (e) {
        var responseObject = JSON.parse(e.target.response);
        var fileInputElement = document.getElementById(responseObject.fileInputElementId);
        fileInputElement.value = responseObject.fileGuid;
        var detailsElement = fileInputElement.parentElement;

        if (e.target.status >= 200 && e.target.status < 300) {
            var message = "Upload of the image is complete. File GUID: " + responseObject.fileGuid;
            processMessage(message, "info", detailsElement);
        } else {
            processErrors(e.target.status, detailsElement);
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