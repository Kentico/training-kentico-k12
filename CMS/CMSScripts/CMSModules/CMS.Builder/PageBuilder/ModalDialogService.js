/**
 * Service for adding and removing background overlay when modal dialog in pagebuilder is opened.
 */
cmsdefine([], function () {
    var MODAL_DIALOG_OVERLAY_ELEMENT_CLASS = 'tmp-modal-dialog-overlay';
    var advancedPopUpHandler = window.top.CMS.AdvancedPopupHandler;
    var screenLockWarningDialogHeight;

    function getOverlayDimensionStrings() {
        var pageBuilderBoundingClientRect = document.querySelector('.page-builder') 
                                            ? document.querySelector('.page-builder').getBoundingClientRect()
                                            : {};
        var topDocumentBoundingClientRect = window.top.document.body.getBoundingClientRect();

        return {
            width: (topDocumentBoundingClientRect.width - pageBuilderBoundingClientRect.width) + 'px',
            height: (topDocumentBoundingClientRect.height - pageBuilderBoundingClientRect.height) + 'px'
        }
    }

    function createOverlayElement(width, height, left) {

        var overlayElement = document.createElement('div');

        overlayElement.classList.add(MODAL_DIALOG_OVERLAY_ELEMENT_CLASS);
        overlayElement.setAttribute('style', 'position: fixed; left: ' + left + '; top: 0' 
            + '; width: ' + width + '; height: ' + height + '; background-color: #696663; opacity: 0.5; z-index: 1;');

        return overlayElement;
    }

    function addModalDialogOverlay() {
        // if overlay already exists do not add another
        if (window.top.document.body.querySelector('.' + MODAL_DIALOG_OVERLAY_ELEMENT_CLASS)) {
            return;
        }

        window.top.document.body.addEventListener('screenLockDialogShow', onScreenLockDialogShow);
        window.top.document.body.addEventListener('screenLockDialogHide', onScreenLockDialogHide);

        var overlayDimensions = getOverlayDimensionStrings();

        var horizontalOverlayElement = createOverlayElement('100%', overlayDimensions.height, overlayDimensions.width);
        var verticalOverlayElement = createOverlayElement(overlayDimensions.width, '100%', '0px');

        // Disable key shortcuts i.e.: F2, etc...
        advancedPopUpHandler.stopPropagation(window.top); 

        window.top.document.body.appendChild(horizontalOverlayElement);
        window.top.document.body.appendChild(verticalOverlayElement);
    } 

    function removeModalDialogOverlay() {
        var overlayElements = window.top.document.body.querySelectorAll('.' + MODAL_DIALOG_OVERLAY_ELEMENT_CLASS);
        
        // overlayElements is a live collection, hence the removal in reverse order
        for (var i = overlayElements.length - 1; i >= 0; i--) {
            window.top.document.body.removeChild(overlayElements[i]);
        }

        // Enable key shortcuts i.e.: F2, etc...
        advancedPopUpHandler.allowPropagation(window.top);

        window.top.document.body.removeEventListener('screenLockDialogShow', onScreenLockDialogShow);
        window.top.document.body.removeEventListener('screenLockDialogHide', onScreenLockDialogHide);
    }

    function onScreenLockDialogShow() {
        screenLockWarningDialogHeight = window.top.document.getElementById('screenLockWarningDialog').clientHeight;
        adjustHorizontalOverlayHeight(screenLockWarningDialogHeight);
    }

    function onScreenLockDialogHide() {
        adjustHorizontalOverlayHeight(screenLockWarningDialogHeight * -1);
    }

    function adjustHorizontalOverlayHeight(offset) {
        var horizontalOverlayElement = window.top.document.body.querySelector('.' + MODAL_DIALOG_OVERLAY_ELEMENT_CLASS);

        var requiredDimensions = getOverlayDimensionStrings();
        if (requiredDimensions.height !== horizontalOverlayElement.clientHeight + 'px') {
            horizontalOverlayElement.style.height = horizontalOverlayElement.clientHeight + offset + 'px';
        }
    }

    return {
        addModalDialogOverlay: addModalDialogOverlay,
        removeModalDialogOverlay: removeModalDialogOverlay,
        createOverlayElement: createOverlayElement,
        MODAL_DIALOG_OVERLAY_ELEMENT_CLASS: MODAL_DIALOG_OVERLAY_ELEMENT_CLASS,
    };
});
