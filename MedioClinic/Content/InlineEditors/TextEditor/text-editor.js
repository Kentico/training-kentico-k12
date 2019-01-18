(function () {
    window.kentico.pageBuilder.registerInlineEditor("text-editor", {
        init: function (options) {
            var editor = options.editor;
            var config = {
                toolbar: {
                  buttons: ["bold", "italic", "underline", "orderedlist", "unorderedlist", "h1", "h2", "h3", "customHtml"]
                },
                imageDragging: false,
                extensions: {
                  imageDragging: {},
                  customHtml: new CustomHtml({
                      buttonText: "Horiz. ruler",
                      htmlToInsert: "<hr />"
                  })
                }
            };

            if (editor.dataset.enableFormatting === "False") {
                config.toolbar = false;
                config.keyboardCommands = false;
            }

            var mediumEditor = new MediumEditor(editor, config);

            mediumEditor.subscribe("editableInput", function () {
                var event = new CustomEvent("updateProperty", {
                    detail: {
                        name: options.propertyName,
                        value: mediumEditor.getContent(),
                        refreshMarkup: false
                    }
                });

                editor.dispatchEvent(event);
            });
        },

        destroy: function (options) {
            var mediumEditor = MediumEditor.getEditorFromElement(options.editor);
            if (mediumEditor) {
                mediumEditor.destroy();
            }
        },

        dragStart: function (options) {
            var mediumEditor = MediumEditor.getEditorFromElement(options.editor);
            var focusedElement = mediumEditor && mediumEditor.getFocusedElement();

            var focusedMediumEditor = focusedElement && MediumEditor.getEditorFromElement(focusedElement);
            var toolbar = focusedMediumEditor && focusedMediumEditor.getExtensionByName("toolbar");

            if (focusedElement && toolbar) {
                toolbar.hideToolbar();
                focusedElement.removeAttribute("data-medium-focused");
            }
        }
    });
})();
