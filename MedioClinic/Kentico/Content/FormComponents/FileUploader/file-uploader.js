window.kentico = window.kentico || {};
window.kentico._forms = window.kentico._forms || {};
window.kentico._forms.formFileUploaderComponent = (function (document) {

    function disableElements(form) {
        form.fileUploaderDisabledElements = [];
        var elements = form.elements;
        for (var i = 0; i < elements.length; i++) {
            var element = elements[i];
            if (!element.disabled) {
                form.fileUploaderDisabledElements.push(i);
                element.disabled = true;
            }
        }
    }

    function enableElements(form) {
        form.fileUploaderDisabledElements.forEach(function (disabledElement) {
            form.elements[disabledElement].disabled = false;
        });
    }

    function clearTempFile(fileInput, inputReplacementFilename, inputPlaceholder, tempFileIdentifierInput, tempFileOriginalNameInput, inputTextButton, inputIconButton) {
        fileInput.value = null;
        fileInput.removeAttribute("hidden");
        inputReplacementFilename.setAttribute("hidden", "hidden");

        inputPlaceholder.innerText = inputPlaceholder.originalText;
        tempFileIdentifierInput.value = "";
        tempFileOriginalNameInput.value = "";

        inputTextButton.setAttribute("hidden", "hidden");
        inputIconButton.setAttribute("data-icon", "select");
        inputIconButton.removeAttribute("title");
    }

    function attachScript(config) {
        var fileInput = document.getElementById(config.fileInputId);
        var inputPlaceholder = document.getElementById(config.fileInputId + "-placeholder");
        var inputReplacementFilename = document.getElementById(config.fileInputId + "-replacement");
        var inputTextButton = document.getElementById(config.fileInputId + "-button");
        var inputIconButton = document.getElementById(config.fileInputId + "-icon");

        var tempFileIdentifierInput = document.getElementById(config.tempFileIdentifierInputId);
        var tempFileOriginalNameInput = document.getElementById(config.tempFileOriginalNameInputId);
        var systemFileNameInput = document.getElementById(config.systemFileNameInputId);
        var originalFileNameInput = document.getElementById(config.originalFileNameInputId);
        var deletePersistentFileInput = document.getElementById(config.deletePersistentFileInputId);

        var deleteFileIconButtonTitle = config.deleteFileIconButtonTitle;

        inputPlaceholder.originalText = inputPlaceholder.innerText;
        inputTextButton.originalText = inputTextButton.innerText;

        // If a file is selected, set text of the label and file input replacement to its filename.
        if (tempFileOriginalNameInput.value || (originalFileNameInput.value && deletePersistentFileInput.value.toUpperCase() === "FALSE")) {
            inputPlaceholder.innerText = tempFileOriginalNameInput.value || config.originalFileNamePlain;

            inputTextButton.removeAttribute("hidden");
            inputIconButton.setAttribute("data-icon", "remove");
            inputIconButton.setAttribute("title", deleteFileIconButtonTitle);

            inputReplacementFilename.removeAttribute("hidden");
            fileInput.setAttribute("hidden", "hidden");
        }

        // If file has not yet been persisted, send a request to delete it.
        var deleteTempFile = function () {
            if (tempFileIdentifierInput.value) {
                var deleteRequest = new XMLHttpRequest();

                deleteRequest.open("POST", config.deleteEndpoint + "&tempFileIdentifier=" + tempFileIdentifierInput.value);
                deleteRequest.send();
            }
        };
        // Deletes both permanent and temp files.
        var deleteFile = function () {
            if (systemFileNameInput.value) {
                deletePersistentFileInput.value = true;
            }

            deleteTempFile();

            clearTempFile(fileInput, inputReplacementFilename, inputPlaceholder, tempFileIdentifierInput, tempFileOriginalNameInput, inputTextButton, inputIconButton);
        };
        // Wrapper for the deleteFile function used when the icon button is clicked.
        var deleteFileIcon = function (event) {
            if (inputIconButton.getAttribute("data-icon") === "remove") {
                event.preventDefault();
                deleteFile();
            }
        };

        inputTextButton.addEventListener("click", deleteFile);
        inputIconButton.addEventListener("click", deleteFileIcon);

        fileInput.addEventListener("change", function () {
            // In IE11 change fires also when setting fileInput value to null.
            if (!fileInput.value) {
                return;
            }

            inputTextButton.removeAttribute("hidden");
            inputIconButton.setAttribute("data-icon", "loading");
            disableElements(fileInput.form);

            // Validate file size.
            var file = fileInput.files[0];
            if (file !== undefined) {
                if (file.size > config.maxFileSize * 1024) {

                    fileInput.value = null;
                    tempFileIdentifierInput.value = "";
                    originalFileNameInput = "";

                    window.alert(config.maxFileSizeExceededErrorMessage);
                    enableElements(fileInput.form);
                    inputIconButton.setAttribute("data-icon", "select");

                    return;
                }
            }

            var data = new FormData();
            var submitRequest = new XMLHttpRequest();
            submitRequest.contentType = "multipart/form-data";

            data.append("file", file);

            submitRequest.addEventListener("load", function (e) {
                if (submitRequest.readyState === 4) {
                    if (submitRequest.status === 200) {
                        var result = submitRequest.response;
                        // IE11 and Edge do not support response type 'json'
                        if (typeof result === "string") {
                            result = JSON.parse(result);
                        }

                        if (result.errorMessage) {
                            fileInput.value = null;
                            alert(result.errorMessage);

                            inputIconButton.setAttribute("data-icon", "select");
                            inputTextButton.setAttribute("hidden", "hidden");
                        } else {
                            if (systemFileNameInput.value) {
                                deletePersistentFileInput.value = true;
                            }
                            deleteTempFile();

                            var filename = fileInput.files[0].name;

                            tempFileIdentifierInput.value = result.fileIdentifier;
                            tempFileOriginalNameInput.value = filename;

                            inputPlaceholder.innerText = filename;
                            inputTextButton.removeAttribute("hidden");
                            inputIconButton.setAttribute("data-icon", "remove");
                            inputIconButton.setAttribute("title", deleteFileIconButtonTitle);

                            inputReplacementFilename.innerText = filename;
                            inputReplacementFilename.removeAttribute("hidden");
                            fileInput.setAttribute("hidden", "hidden");
                        }
                    } else {
                        alert("Error sending file: " + submitRequest.statusText);

                        inputIconButton.setAttribute("data-icon", "select");
                        inputTextButton.setAttribute("hidden", "hidden");
                    }

                    inputTextButton.innerHTML = inputTextButton.originalText;
                    enableElements(fileInput.form);
                }
            });

            submitRequest.upload.addEventListener("progress", function (event) {
                inputTextButton.innerText = parseInt(event.loaded / event.total * 100) + "%";
            });

            submitRequest.open("POST", config.submitEndpoint);
            submitRequest.responseType = "json";
            submitRequest.send(data);
        });
    }

    return {
        attachScript: attachScript
    };
}(document));
