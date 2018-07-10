cmsdefine([
    'CMS.PageBuilder/MessageService',
    'CMS.PageBuilder/DragAndDropService',
    'CMS.PageBuilder/MessageTypes',
], function (msgService, dndService, messageTypes) {

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

            if (event.data.msg === messageTypes.CONFIGURATION_STORED) {
                eval(originalScript);
            }
            else if (event.data.msg === messageTypes.CONFIGURATION_CHANGED) {
                window.CMSContentManager && window.CMSContentManager.changed(true);
                window.top.CancelScreenLockCountdown && window.top.CancelScreenLockCountdown();
            }
            else if (event.data.msg === messageTypes.MESSAGING_ERROR) {
                msgService.showError(event.data.data, true);
            }
            else if (event.data.msg === messageTypes.MESSAGING_EXCEPTION) {
                msgService.showError(event.data.data);
            }
            else if (event.data.msg === messageTypes.MESSAGING_WARNING) {
                msgService.showWarning(event.data.data);
            }
            else if (event.data.msg === messageTypes.MESSAGING_DRAG_START) {
                dndService.addDnDCancellationEvents();
            }
            else if (event.data.msg === messageTypes.MESSAGING_DRAG_STOP) {
                dndService.removeDnDCancellationEvents();
            }
        };

        var registerListener = function () {
            window.addEventListener('message', receiveMessage);
        };

        var saveConfiguration = function (script) {
            if (frameLoaded === false) return;

            if (contentModified === true) {
                originalScript = script;
                frame.contentWindow.postMessage({ msg: messageTypes.SAVE_CONFIGURATION, guid: instanceGuid }, targetOrigin);
            } else {
                eval(script);
            }
        };

        var bindSaveChanges = function () {
            window.CMSContentManager && window.CMSContentManager.eventManager.on('contentChanged', function (event, isModified) {
                contentModified = isModified;
            });
        };

        var bindFrameLoad = function () {
            frame.addEventListener('load', function () {
                frameLoaded = true;
            });
        };

        var handleFrameHeight = function () {
            var resize = function () {
                var panel = document.getElementsByClassName('preview-edit-panel')[0];
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
