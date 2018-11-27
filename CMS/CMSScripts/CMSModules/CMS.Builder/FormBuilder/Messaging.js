cmsdefine([
    'CMS.Builder/MessageService',
    'CMS.Builder/MessageTypes'
], function (msgService, messageTypes) {

    var Module = function (serverData) {
        var frame = document.getElementById(serverData.frameId);
        var targetOrigin = serverData.origin;

        var receiveMessage = function (event) {            
            if (event.origin !== targetOrigin) {
                return;
            }

            switch (event.data.msg) {
                case messageTypes.MESSAGING_ERROR:
                    msgService.showError(event.data.data, true);
                    frame.src = "about:blank";
                    break;

                case messageTypes.MESSAGING_EXCEPTION:
                    msgService.showError(event.data.data);
                    frame.src = "about:blank";
                    break;

                case messageTypes.MESSAGING_WARNING:
                    msgService.showWarning(event.data.data);
                    break;
                case messageTypes.CONFIGURATION_CHANGED:
                    window.top.CancelScreenLockCountdown && window.top.CancelScreenLockCountdown();
                    break;
            }
        };

        var registerPostMessageListener = function () {
            window.addEventListener('message', receiveMessage);
        };

        registerPostMessageListener();

        window.CMS = window.CMS || {};
    };

    return Module;
});
