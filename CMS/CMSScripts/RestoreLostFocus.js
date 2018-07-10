var lastFocusedControlId = "";

function focusHandler(e) {
    if ((e.originalTarget != undefined) && (e.originalTarget != null)) {
        try {
            if (!e.originalTarget.focus) {
                e.originalTarget.focus = true;
            }
        }
        catch (err) { }
    }
}

function appInit() {
    if (typeof (window.addEventListener) !== "undefined") {
        window.addEventListener("focus", focusHandler, true);
    }
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoading(pageLoadingHandler);
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoadedHandler);
}

function pageLoadingHandler(sender, args) {
    lastFocusedControlId = typeof (document.activeElement) === "undefined"
        ? "" : document.activeElement.id;
}

function focusControl(targetControl) {
    if (Sys.Browser.agent === Sys.Browser.InternetExplorer) {
        var focusTarget = targetControl;
        if (focusTarget && (typeof (focusTarget.contentEditable) !== "undefined")) {
            oldContentEditableSetting = focusTarget.contentEditable;
            focusTarget.contentEditable = false;
        }
        else {
            focusTarget = null;
        }

        try
        {
            targetControl.focus();
            if (focusTarget) {
                focusTarget.contentEditable = oldContentEditableSetting;
            }
        }
        catch (err) { } 
    }
    else {
        try {
            targetControl.focus();
        }
        catch (err) { }
    }
}

function pageLoadedHandler(sender, args) {
    if (typeof (lastFocusedControlId) !== "undefined" && lastFocusedControlId != "") {
        var newFocused = $get(lastFocusedControlId);
        if ((newFocused != null) && (newFocused != undefined) && (newFocused.disabled != undefined) && (newFocused.disabled != null) && (newFocused.disabled == false)) {
            focusControl(newFocused);
        } 
    }
}

Sys.Application.add_init(appInit);