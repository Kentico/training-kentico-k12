var renderFileDetails = function (target) {
    var mbSize = 1048576;
    var file = target.files[0];

    if (file) {
        var fileSize = 0;

        if (file.size > mbSize) {
            fileSize = (Math.round(file.size * 100 / mbSize) / 100).toString() + 'MB';
        } else {
            fileSize = (Math.round(file.size * 100 / 1024) / 100).toString() + 'kB';
        }

        var detailsElement = target.parentElement.parentElement.querySelector(".kn-upload-file-details");
        detailsElement.querySelector(".kn-file-size").innerHTML = "<strong>Size:</strong> " + fileSize;
        detailsElement.querySelector(".kn-file-type").innerHTML = "<strong>Type:</strong> " + file.type;
    }
};

var uploadFile = function (target, url) {
    if (url.length > 0) {
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

var getParentForm = function (target, bodyElement) {
    if (!bodyElement || bodyElement === null) {
        bodyElement = document.getElementsByTagName("body")[0];
    }

    if (target !== bodyElement) {
        var parent = target.parentElement;

        if (parent.tagName === "FORM") {
            return parent;
        } else {
            return getParentForm(parent, bodyElement);
        }
    } else {
        return null;
    }
};

var onUploadCompleted = function (e) {
    var responseObject = JSON.parse(e.target.response);
    var filePathElement = document.getElementById(responseObject.filePathId);
    filePathElement.value = responseObject.fileGuid;
    console.info("Upload of the image is complete. File GUID: " + responseObject.fileGuid);
};

var onUploadProgressChange = function (e) {
    var percentComplete = Math.round(e.loaded * 100 / e.total);
    console.info("Upload progress: " + percentComplete + "%");
};

var onUploadFailed = function (e) {
    console.error("The upload of the image failed.");
}