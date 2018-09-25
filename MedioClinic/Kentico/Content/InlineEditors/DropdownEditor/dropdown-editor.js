(function () {
    window.kentico.pageBuilder.registerInlineEditor("kentico-dropdown-editor", {
        init: function (options) {
            var editor = options.editor;
            var dropdown = editor.querySelector(".ktc-selector");
            dropdown.addEventListener("change", function () {
                var event = new CustomEvent("updateProperty", {
                    detail: {
                        value: dropdown.value,
                        name: options.propertyName
                    }
                });
                editor.dispatchEvent(event);
            });
        }
    });
})();
