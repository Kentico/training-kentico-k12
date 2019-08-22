window.kentico = window.kentico || {};

/**
 * Modal dialog API type definition.
 * @typedef {Object} ModalDialog
 * @property {Function} open Opens a modal dialog.
 * @property {Function} save Saves currently opened modal dialog.
 * @property {Function} close Closes currently opened modal dialog.
 * @property {Function} getData Gets the data for currently opened modal dialog.
 */
(function (localKenticoNamespace, parentKenticoNamespace) {
    localKenticoNamespace.modalDialog = localKenticoNamespace.modalDialog || {};

    registerModalDialogApi(localKenticoNamespace.modalDialog, parentKenticoNamespace.modalDialog);
    registerLocalizationApi();

    /**
     * Registers the modal dialog API in current window.
     * @param {ModalDialog} modalDialog Modal dialog service object.
     * @param {ModalDialog} parentModalDialog Modal dialog service object in parent window.
     */
    function registerModalDialogApi(modalDialog, parentModalDialog) {
        modalDialog.open = parentModalDialog.open;
        modalDialog.apply = parentModalDialog.apply.bind(null, window);
        modalDialog.cancel = parentModalDialog.cancel.bind(null, window);
        modalDialog.getData = parentModalDialog.getData;
        modalDialog.resize = parentModalDialog.resize;
    }

    /**
     * Registers the localization API in current window.
     */
    function registerLocalizationApi() {
        localKenticoNamespace.localization = parentKenticoNamespace.localization;
    }
}(window.kentico, window.parent.kentico));
