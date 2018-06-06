function editDocumentWithProgID(strDocument, strOpenDocumentError, strEditDocumentError) {
    var EditDocumentButton = null;
    var office2007NotInstalled = false;

    try {
        // Create object for Office 2007
        EditDocumentButton = new ActiveXObject("SharePoint.OpenDocuments.3");
    }
    catch (exception) {
        office2007NotInstalled = true;
    }

    if (office2007NotInstalled) {
        try {
            // Create object for Office 2003
            EditDocumentButton = new ActiveXObject("SharePoint.OpenDocuments.2");
        }
        catch (exception) {
        }
    }

    if (EditDocumentButton != null) {
        if (strDocument.charAt(0) == "/" || strDocument.substr(0, 3).toLowerCase() == "%2f") {
            strDocument = document.location.protocol + "//" + document.location.host + strDocument;
        }
        if (!EditDocumentButton.EditDocument(strDocument)) {
            alert(strEditDocumentError);
        }
    }
    else {
        alert(strOpenDocumentError);
    }
}

