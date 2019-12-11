window.kentico = window.kentico || {};

/**
 * Page selector module.
 * @param {object} namespace Namespace under which this module operates.
 */
(function (namespace) {
    // Register the initialization function only in page builder
    if (!namespace.pageBuilder) {
        return;
    }

    var init = function (id, selectedPageData) {
        var component = document.getElementById(id);
        component.getString = window.kentico.localization.getString;
        component.selectedPageData = selectedPageData;
    };

    const modalDialogInternal = namespace._modalDialog = namespace._modalDialog || {};
    const pageSelector = modalDialogInternal.pageSelector = modalDialogInternal.pageSelector || {};
    pageSelector.init = init;
})(window.kentico);