/**
* Returns current page address (URL).
*/
cmsdefine([], function () {

    /**
     * Gets current URL.
     *
     * @returns {string} Current URL
     */
    var getCurrentUrl = function () {
        return window.location.href;
    };

    return {
        getCurrentUrl: getCurrentUrl
    };
});
