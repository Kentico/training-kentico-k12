function getFlashVersion() {
    // IE
    try {
        try {
            // Avoid fp6 minor version lookup issues
            var axo = new ActiveXObject('ShockwaveFlash.ShockwaveFlash.6');
            try { axo.AllowScriptAccess = 'always'; }
            catch (e) { return '6,0,0'; }
        } catch (e) { }
        return new ActiveXObject('ShockwaveFlash.ShockwaveFlash').GetVariable('$version').replace(/\D+/g, ',').match(/^,?(.+),?$/)[1];
        // Other browsers
    } catch (e) {
        try {
            if (navigator.mimeTypes["application/x-shockwave-flash"].enabledPlugin) {
                return (navigator.plugins["Shockwave Flash 2.0"] || navigator.plugins["Shockwave Flash"]).description.replace(/\D+/g, ",").match(/^,?(.+),?$/)[1];
            }
        } catch (e) { }
    }
    return '0,0,0';
}

function getJava() {
    var enabled = false;
    if (typeof navigator != 'undefined' && typeof navigator.javaEnabled != 'undefined')
        enabled = navigator.javaEnabled();

    return enabled;
}

function getOS() {
    var OSName = "0";
    if (navigator.appVersion.indexOf("Win") != -1) OSName = "1";
    if (navigator.appVersion.indexOf("Mac") != -1) OSName = "2";
    if (navigator.appVersion.indexOf("X11") != -1) OSName = "3";
    if (navigator.appVersion.indexOf("Linux") != -1) OSName = "4";
    if (navigator.appVersion.indexOf("SunOS") != -1) OSName = "5";

    return OSName;
}

function getSilverlight() {
    var SLVersion;
    try {
        try {
            // IE support
            var control = new ActiveXObject('AgControl.AgControl');
            if (control.IsVersionSupported("4.0"))
                SLVersion = 4;
            else
                if (control.IsVersionSupported("3.0"))
                SLVersion = 3;
            else
                if (control.IsVersionSupported("2.0"))
                SLVersion = 2;
            else
                SLVersion = 1;
            control = null;
        }

        catch (e) {
            // Other browsers
            var plugin = navigator.plugins["Silverlight Plug-In"];
            if (plugin) {
                if (plugin.description === "1.0.30226.2")
                    SLVersion = 2;
                else
                    SLVersion = parseInt(plugin.description[0]);
            }
            else
                SLVersion = 0;
        }
    }

    catch (e) {
        SLVersion = 0;
    }
    return SLVersion;
}

function collectBrowserData(logRes, logColor, logOS, logSilvelight, logJava, logFlash, url, guid) {
    var ret;

    ret = logRes ? screen.width + ";" + screen.height : ';';
    ret += ";" + (logColor ? screen.colorDepth : '');
    ret += ";" + (logOS ? getOS() : '');
    ret += ";" + (logSilvelight ? getSilverlight() : '');
    ret += ";" + (logJava ? getJava() : '');
    ret += ";" + (logFlash ? getFlashVersion().split(',').shift() : '');

    // Send async request to blank page to store to DB
    var client = null;
    try {
        if (window.XMLHttpRequest) {
            client = new XMLHttpRequest();
        }
        else {
            client = new ActiveXObject("Microsoft.XMLHTTP");
        }

        client.open("POST", url + '?data=' + ret + '&guid=' + guid);
        client.setRequestHeader("Content-Type", "text/plain;charset=UTF-8");
        client.setRequestHeader("X-Requested-With", "XMLHttpRequest");
        client.send('');
    }
    catch (e) { }
    
    
}



