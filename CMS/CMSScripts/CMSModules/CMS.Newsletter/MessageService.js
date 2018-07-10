/**
 * Service for showing the information messages.
 */
cmsdefine(['jQuery'], function ($) {

    var TIMEOUT = 2000;

    function showSuccess(msg) {
        var $successMsg = $('.alert-success-floating');
        $successMsg.removeClass('hidden')
                   .find('.alert-label')
                   .text(msg);

        setTimeout(function() {
            $successMsg.addClass('hidden');
        }, TIMEOUT);
    }

    function showError(msg) {
        $('.alert-error-floating').removeClass('hidden')
                                  .find('.alert-label')
                                  .text(msg);
    }

    function showInfo(msg) {
        $('.alert-info-floating').removeClass('hidden')
                                 .find('.alert-label')
                                 .text(msg);
    }

    function showInfoRight(msg) {
        $('.alert-info-floating-right').removeClass('hidden')
            .find('.alert-label')
            .text(msg);
    }

    function hideInfoRight() {
        $('.alert-info-floating-right').addClass('hidden')
            .find('.alert-label')
            .text(" ");
    }

    return {
        showSuccess: showSuccess,
        showError: showError,
        showInfo: showInfo,
        showInfoRight: showInfoRight,
        hideInfoRight: hideInfoRight
    };
});