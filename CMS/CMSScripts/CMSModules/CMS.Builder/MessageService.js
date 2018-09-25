/**
 * Service for showing the Page builder information messages.
 */
cmsdefine(['jQuery'], function ($) {

    var errorElm = $('.alert-error-absolute');
    var warningElm = $('.alert-warning-absolute');

    function showWarning(msg) {
        showMessage(warningElm, msg, true);
    }

    function showError(msg, append) {
        showMessage(errorElm, msg, append);
    }

    function showMessage(elem, msg, append) {
        elem.removeClass('hidden');
        var label = elem.find('.alert-label');

        append && label.text().trim() ? label.append(msg) : label.text(msg);
        label.append('<br />');
    }

    function ensureCloseButton(elem) {
        var closeElem = $('<span></span>').addClass('alert-close');
        closeElem.append($('<i></i>').addClass('close icon-modal-close').click(function () {
            elem.addClass('hidden');
        }));
        closeElem.append($('<span>Close</span>').addClass('sr-only'));

        elem.append(closeElem);
    }

    ensureCloseButton(errorElm);
    ensureCloseButton(warningElm);

    return {
        showWarning: showWarning,
        showError: showError
    };
});
