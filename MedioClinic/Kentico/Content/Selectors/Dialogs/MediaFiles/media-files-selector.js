(function () {
  var init = function () {
    var component = document.getElementsByTagName("kentico-media-files")[0];
    var data = window.kentico.modalDialog.getData();
    component.getString = window.kentico.localization.getString;
    component.values = data.selectedValues;
    component.libraryName = data.libraryName;
    component.maxFilesLimit = data.maxFilesLimit;
    component.allowedExtensions = data.allowedExtensions;
  };

  const kenticoNamespace = window.kentico || {};
  const modalDialog = kenticoNamespace.modalDialog = kenticoNamespace.modalDialog || {};
  const mediaFilesSelector = modalDialog.mediaFilesSelector = modalDialog.mediaFilesSelector || {};
  const mediaFilesSelectorDialog = mediaFilesSelector.dialog = mediaFilesSelector.dialog || {};
  mediaFilesSelectorDialog._init = init;
})();