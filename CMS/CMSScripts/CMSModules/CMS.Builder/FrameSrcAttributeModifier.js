/**
* Modifies frame 'src' attribute and adds administration domain into it.
*/
cmsdefine(["CMS/UrlHelper", "CMS/CurrentUrlHelper", "CMS.Builder/Constants"], function (urlHelper, currentUrlHelper, constants) {

    // Module constructor
    var Module = function (serverData) {
        serverData = serverData || {};

        if (serverData.frameId && serverData.frameSrc) {

            var frame = document.getElementById(serverData.frameId);

            if (frame) {
                frame.setAttribute("src", urlHelper.addParameterToUrl(
                    serverData.frameSrc,
                    constants.ADMINISTRATION_DOMAIN_PARAMETER_NAME,
                    urlHelper.getHostWithScheme(currentUrlHelper.getCurrentUrl()))
                );
            }
        }
    };

    return Module;
});