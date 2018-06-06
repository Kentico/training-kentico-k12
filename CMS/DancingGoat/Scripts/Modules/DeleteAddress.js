define(['jQuery'], function ($) {

    var showConfirmation = function (elem) {
            // Ensure hiding of other confirmation messages
            $('.confirmation-wrapper').hide();
            $('.inner-div').removeClass('delete-confirm');

            // Get wrapper and display confirmation
            var $addressTile = $(elem).closest('.inner-div');
            var $confirmMsg = $addressTile.children('.confirmation-wrapper');

            $addressTile.addClass('delete-confirm');
            $confirmMsg.slideToggle('fast');
        },

        hideConfirmation = function (elem) {
            var $addressTile = $(elem).closest('.inner-div');
            var $confirmMsg = $addressTile.children('.confirmation-wrapper');

            // Hide confirmation message
            $addressTile.removeClass('delete-confirm');
            $confirmMsg.slideToggle('fast');
        },

        delayDelete = function (e, elem) {
            e.preventDefault();
            var $linkBtn = $(elem);

            $linkBtn.closest('.address-tile').hide('fast', function () {

                // Trigger a postback for link button after animation
                location.replace($linkBtn.attr('href'));
            });
        };

    $('.address-row').on('click', '.delete-button', function () {
        showConfirmation(this);
    });

    $('.address-row').on('click', '.cancel-button', function () {
        hideConfirmation(this);
    });

    $('.address-row').on('click', '.confirm-delete-button', function () {
        delayDelete(event, this);
    });
});