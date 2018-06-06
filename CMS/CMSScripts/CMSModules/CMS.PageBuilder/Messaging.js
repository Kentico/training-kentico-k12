cmsdefine([], function () {

    var Module = function (serverData) {
        var frameLoaded = false;
        var contentModified = false;
        var frame = document.getElementById(serverData.frameId);
        var instanceGuid = serverData.guid;
        var targetOrigin = serverData.origin;
        var originalScript;


        var receiveMessage = function (event) {
            if (event.origin !== targetOrigin) {
                return;
            }

            if (event.data.msg === "Kentico.ConfigurationStored") {
                eval(originalScript);
            }
            else if (event.data.msg === "Kentico.ConfigurationChanged") {
                window.CMSContentManager && window.CMSContentManager.changed(true);
            }
        };

        var registerListener = function () {
            window.addEventListener("message", receiveMessage);
        };

        var saveConfiguration = function (script) {
            if (frameLoaded === false) return;

            if (contentModified === true) {
                originalScript = script;
                frame.contentWindow.postMessage({ msg: "Kentico.SaveConfiguration", guid: instanceGuid }, targetOrigin);
            } else {
                eval(script);
            }
        };

        var bindSaveChanges = function () {
            window.CMSContentManager && window.CMSContentManager.eventManager.on("contentChanged", function (event, isModified) {
                contentModified = isModified;
            });
        };

        var bindFrameLoad = function () {
            frame.addEventListener("load", function () {
                frameLoaded = true;
            });
        };

        var handleFrameHeight = function () {
            var resize = function () {
                var panel = document.getElementsByClassName("preview-edit-panel")[0];
                if (panel != null) {
                    var height = document.body.offsetHeight - panel.offsetHeight;
                    frame.height = height;
                }
            };

            // Use jQuery to handle cross-browser compatibility
            $cmsj(window).bind('resize', resize);
            $cmsj(document).ready(resize);
        };

        handleFrameHeight();
        bindSaveChanges();
        bindFrameLoad();
        registerListener();


        window.CMS = window.CMS || {};
        var pageBuilder = window.CMS.PageBuilder = window.CMS.PageBuilder || {};
        pageBuilder.save = saveConfiguration;
    };

    return Module;
});